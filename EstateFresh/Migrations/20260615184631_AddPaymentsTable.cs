using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstateFresh.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PayFastTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 1,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 15));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 2,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 15));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 3,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 15));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 4,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 16));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 5,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 16));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 6,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 16));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 7,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 17));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 8,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 17));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 9,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 17));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 10,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 18));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 11,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 18));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 12,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 18));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 13,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 19));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 14,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 19));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 15,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 19));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 16,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 20));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 17,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 20));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 18,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 20));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 19,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 21));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 20,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 21));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 21,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 21));

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderID",
                table: "Payments",
                column: "OrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 1,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 10));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 2,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 10));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 3,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 10));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 4,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 11));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 5,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 11));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 6,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 11));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 7,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 12));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 8,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 12));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 9,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 12));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 10,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 13));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 11,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 13));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 12,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 13));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 13,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 14));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 14,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 14));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 15,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 14));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 16,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 15));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 17,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 15));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 18,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 15));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 19,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 16));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 20,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 16));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 21,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 16));
        }
    }
}
