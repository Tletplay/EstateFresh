using EstateFresh.Data;
using EstateFresh.Models;
using EstateFresh.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EstateFresh.Controllers;

public class PaymentController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IDriverAssignmentService _driverAssignment;

    public PaymentController(ApplicationDbContext db, IDriverAssignmentService driverAssignment)
    {
        _db = db;
        _driverAssignment = driverAssignment;
    }

    // GET: Payment/PayDeposit
    public async Task<IActionResult> PayDeposit(int orderId)
    {
        var userId = HttpContext.Session.GetString("UserID");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var order = await _db.Orders
            .FirstOrDefaultAsync(o => o.OrderID == orderId && o.CustomerID == int.Parse(userId));

        if (order == null)
        {
            TempData["ErrorMessage"] = "Order not found.";
            return RedirectToAction("BookPickup", "Order");
        }

        ViewBag.OrderId = order.OrderID;
        ViewBag.CustomerEmail = HttpContext.Session.GetString("UserEmail");

        return View();
    }

    // GET: Payment/Confirm
    public async Task<IActionResult> Confirm(int orderId, string? payment_status, string? pf_payment_id)
    {
        var order = await _db.Orders.FindAsync(orderId);

        // ⚠️ TEMPORARY DEV-ONLY SHORTCUT ⚠️
        // PayFast's real confirmation only arrives via Notify (server-to-server),
        // which can't reach localhost during local testing. Until we set up a
        // public tunnel (e.g. ngrok) or deploy, we treat reaching this page as
        // success so the rest of the app can be tested. We also now read the
        // order ID from our own return_url instead of relying on PayFast's
        // m_payment_id, since PayFast doesn't attach it to browser redirects.
        // MUST BE REMOVED / REPLACED before accepting real payments.
        if (order != null)
        {
            // Check if payment already recorded
            var existingPayment = await _db.Payments
                .FirstOrDefaultAsync(p => p.OrderID == orderId && p.Type == "Deposit");

            if (existingPayment == null)
            {
                var payment = new Payment
                {
                    OrderID = orderId,
                    Amount = 50,
                    Type = "Deposit",
                    PayFastTransactionId = pf_payment_id,
                    Status = "Paid",
                    PaidAt = DateTime.UtcNow
                };
                _db.Payments.Add(payment);
                order.Status = "Awaiting Pickup";
                await _db.SaveChangesAsync();
                await _driverAssignment.AssignDriverAsync(order.OrderID);
            }

            TempData["SuccessMessage"] = "Deposit paid successfully! Your order is confirmed.";
        }
        else
        {
            TempData["ErrorMessage"] = "Payment was not completed. Please try again.";
        }

        return RedirectToAction("MyOrders", "Order");
    }

    // GET: Payment/Cancel
    public IActionResult Cancel()
    {
        TempData["ErrorMessage"] = "Payment was cancelled.";
        return RedirectToAction("BookPickup", "Order");
    }

    // POST: Payment/Notify
    [HttpPost]
    public async Task<IActionResult> Notify(IFormCollection form)
    {
        var paymentStatus = form["payment_status"].ToString();
        var orderId = int.Parse(form["m_payment_id"].ToString());
        var pfPaymentId = form["pf_payment_id"].ToString();

        if (paymentStatus == "COMPLETE")
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order != null)
            {
                var existingPayment = await _db.Payments
                    .FirstOrDefaultAsync(p => p.OrderID == orderId && p.Type == "Deposit");

                if (existingPayment == null)
                {
                    var payment = new Payment
                    {
                        OrderID = orderId,
                        Amount = 50,
                        Type = "Deposit",
                        PayFastTransactionId = pfPaymentId,
                        Status = "Paid",
                        PaidAt = DateTime.UtcNow
                    };
                    _db.Payments.Add(payment);
                    order.Status = "Awaiting Pickup";
                    await _db.SaveChangesAsync();
                    await _driverAssignment.AssignDriverAsync(order.OrderID);
                }
            }
        }

        return Ok();
    }

    // GET: Payment/PayBalance
    public async Task<IActionResult> PayBalance(int orderId)
    {
        var userId = HttpContext.Session.GetString("UserID");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var order = await _db.Orders
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.OrderID == orderId && o.CustomerID == int.Parse(userId));

        if (order == null || !order.BalanceDue.HasValue || order.BalanceDue.Value <= 0)
        {
            TempData["ErrorMessage"] = "No balance due for this order.";
            return RedirectToAction("MyOrders", "Order");
        }

        var model = new PayBalanceViewModel
        {
            OrderID = order.OrderID,
            BalanceDue = order.BalanceDue.Value,
            CustomerEmail = order.Customer?.Email ?? ""
        };

        return View(model);
    }

    
    // GET: Payment/BalanceConfirm
    public async Task<IActionResult> BalanceConfirm(int orderId, string? payment_status, string? pf_payment_id)
    {
        var order = await _db.Orders.FindAsync(orderId);

        // ⚠️ TEMPORARY DEV-ONLY SHORTCUT ⚠️
        // Same reasoning as Confirm: PayFast doesn't attach payment details to
        // browser redirects locally, so we read orderId from our own return_url
        // and treat reaching this page as success for local testing.
        // MUST BE REMOVED / REPLACED before accepting real payments.
        if (order != null && order.BalanceDue.HasValue && order.BalanceDue.Value > 0)
        {
            var existingPayment = await _db.Payments
                .FirstOrDefaultAsync(p => p.OrderID == orderId && p.Type == "Balance");

            if (existingPayment == null)
            {
                var payment = new Payment
                {
                    OrderID = orderId,
                    Amount = order.BalanceDue ?? 0,
                    Type = "Balance",
                    PayFastTransactionId = pf_payment_id,
                    Status = "Paid",
                    PaidAt = DateTime.UtcNow
                };
                _db.Payments.Add(payment);
                order.BalancePaid = true;
                order.Status = "Completed";
                await _db.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Balance paid successfully! Order completed.";
        }
        else
        {
            TempData["ErrorMessage"] = "Payment failed. Please try again.";
        }

        return RedirectToAction("MyOrders", "Order");
    }

    // POST: Payment/BalanceNotify
    [HttpPost]
    public async Task<IActionResult> BalanceNotify(IFormCollection form)
    {
        var paymentStatus = form["payment_status"].ToString();
        var orderId = int.Parse(form["m_payment_id"].ToString());
        var pfPaymentId = form["pf_payment_id"].ToString();

        if (paymentStatus == "COMPLETE")
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order != null)
            {
                var existingPayment = await _db.Payments
                    .FirstOrDefaultAsync(p => p.OrderID == orderId && p.Type == "Balance");

                if (existingPayment == null)
                {
                    var payment = new Payment
                    {
                        OrderID = orderId,
                        Amount = order.BalanceDue ?? 0,
                        Type = "Balance",
                        PayFastTransactionId = pfPaymentId,
                        Status = "Paid",
                        PaidAt = DateTime.UtcNow
                    };
                    _db.Payments.Add(payment);
                    order.BalancePaid = true;
                    order.Status = "Completed";
                    await _db.SaveChangesAsync();
                }
            }
        }

        return Ok();
    }
}