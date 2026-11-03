using EstateFresh.Data;
using EstateFresh.Models;
using EstateFresh.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstateFresh.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly IEmailService _emailService;

    public AccountController(ApplicationDbContext db, IEmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }

    // GET: Account/Login
    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("UserEmail") != null)
        {
            return RedirectToAction("Index", "Dashboard");
        }
        return View();
    }

    // POST: Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user == null)
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        if (!user.IsActive)
        {
            ModelState.AddModelError("", "Your account has been deactivated.");
            return View(model);
        }

        if (!user.EmailVerified)
        {
            ModelState.AddModelError("", "Please verify your email before logging in.");
            return View(model);
        }

        if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        // Set session
        HttpContext.Session.SetString("UserID", user.UserID.ToString());
        HttpContext.Session.SetString("UserEmail", user.Email);
        HttpContext.Session.SetString("FullName", user.FullName);
        HttpContext.Session.SetString("Role", user.Role);

        // Redirect based on role
        return user.Role switch
        {
            "Driver" => RedirectToAction("Dashboard", "Driver"),
            "Manager" => RedirectToAction("Index", "Manager"),
            "Admin" => RedirectToAction("Index", "Admin"),
            _ => RedirectToAction("Index", "Dashboard")
        };
    }

    // GET: Account/Register
    public IActionResult Register()
    {
        if (HttpContext.Session.GetString("UserEmail") != null)
        {
            return RedirectToAction("Index", "Dashboard");
        }
        return View();
    }

    // POST: Account/Register
    // POST: Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Check if email exists
        var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (existingUser != null)
        {
            ModelState.AddModelError("Email", "Email already registered. Please login.");
            return View(model);
        }

        // Generate verification token
        var verificationToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", "_").Replace("+", "-");

        // Create new user
        var user = new User
        {
            Email = model.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
            FullName = model.FullName,
            Phone = model.Phone,
            Role = "Customer",
            EmailVerified = false,
            VerificationToken = verificationToken,
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // Send verification email
        var verifyLink = Url.Action("VerifyEmail", "Account",
            new { token = verificationToken, email = user.Email }, Request.Scheme);

        var emailBody = $@"
        <h2>Welcome to Estate Fresh!</h2>
        <p>Please verify your email address by clicking the link below:</p>
        <a href='{verifyLink}' style='background-color: #5F7D69; color: white; padding: 10px 20px; text-decoration: none; border-radius: 8px;'>Verify Email</a>
        <p>If you didn't create an account, please ignore this email.</p>
        <hr />
        <small>Estate Fresh - Premium Laundry Concierge</small>
    ";

        await _emailService.SendEmailAsync(user.Email, "Verify Your Email - Estate Fresh", emailBody);

        TempData["SuccessMessage"] = "Registration successful! Please check your email to verify your account.";
        return RedirectToAction("Login");
    }


    // GET: Account/ForgotPassword
    public IActionResult ForgotPassword()
    {
        if (HttpContext.Session.GetString("UserEmail") != null)
        {
            return RedirectToAction("Index", "Dashboard");
        }
        return View();
    }

    // POST: Account/ForgotPassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user != null)
        {
            // Generate reset token
            var resetToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
                .Replace("/", "_").Replace("+", "-");

            user.ResetToken = resetToken;
            user.ResetTokenExpiry = DateTime.UtcNow.AddHours(24);
            await _db.SaveChangesAsync();

            // Build reset link
            var resetLink = Url.Action("ResetPassword", "Account",
                new { token = resetToken, email = user.Email }, Request.Scheme);

            // Send email
            var emailBody = $@"
            <h2>Reset Your Password</h2>
            <p>Click the link below to reset your password. This link expires in 24 hours.</p>
            <a href='{resetLink}' style='background-color: #5F7D69; color: white; padding: 10px 20px; text-decoration: none; border-radius: 8px;'>Reset Password</a>
            <p>If you didn't request this, please ignore this email.</p>
            <hr />
            <small>Estate Fresh - Premium Laundry Concierge</small>
        ";

            await _emailService.SendEmailAsync(user.Email, "Reset Your Password - Estate Fresh", emailBody);

            TempData["SuccessMessage"] = "Password reset link has been sent to your email.";
        }
        else
        {
            // Don't reveal that email doesn't exist (security best practice)
            TempData["SuccessMessage"] = "If your email is registered, you will receive a password reset link.";
        }

        return RedirectToAction("Login");
    }

    // GET: Account/ResetPassword
    public async Task<IActionResult> ResetPassword(string token, string email)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
        {
            TempData["ErrorMessage"] = "Invalid password reset link.";
            return RedirectToAction("Login");
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || user.ResetToken != token || user.ResetTokenExpiry < DateTime.UtcNow)
        {
            TempData["ErrorMessage"] = "Invalid or expired password reset link.";
            return RedirectToAction("Login");
        }

        var model = new ResetPasswordViewModel
        {
            Token = token,
            Email = email
        };

        return View(model);
    }

    // POST: Account/ResetPassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user == null || user.ResetToken != model.Token || user.ResetTokenExpiry < DateTime.UtcNow)
        {
            TempData["ErrorMessage"] = "Invalid or expired password reset link.";
            return RedirectToAction("Login");
        }

        // Update password
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
        user.ResetToken = null;
        user.ResetTokenExpiry = null;
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = "Password reset successfully! Please login with your new password.";
        return RedirectToAction("Login");
    }

    // POST: Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    // GET: Account/AccessDenied
    public IActionResult AccessDenied()
    {
        return View();
    }

    // GET: Account/VerifyEmail
    public async Task<IActionResult> VerifyEmail(string token, string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email && u.VerificationToken == token);

        if (user == null)
        {
            TempData["ErrorMessage"] = "Invalid verification link.";
            return RedirectToAction("Login");
        }

        user.EmailVerified = true;
        user.VerificationToken = null;
        await _db.SaveChangesAsync();

        TempData["SuccessMessage"] = "Email verified successfully! You can now login.";
        return RedirectToAction("Login");
    }
}