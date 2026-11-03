using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EstateFresh.Migrations
{
    /// <inheritdoc />
    public partial class AddServicesAndSlots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DeliverySlots",
                columns: new[] { "SlotID", "EndTime", "MaxOrders", "SlotDate", "StartTime" },
                values: new object[,]
                {
                    { 1, new TimeOnly(12, 0, 0), 10, new DateOnly(2026, 6, 10), new TimeOnly(9, 0, 0) },
                    { 2, new TimeOnly(16, 0, 0), 10, new DateOnly(2026, 6, 10), new TimeOnly(13, 0, 0) },
                    { 3, new TimeOnly(20, 0, 0), 10, new DateOnly(2026, 6, 10), new TimeOnly(17, 0, 0) },
                    { 4, new TimeOnly(12, 0, 0), 10, new DateOnly(2026, 6, 11), new TimeOnly(9, 0, 0) },
                    { 5, new TimeOnly(16, 0, 0), 10, new DateOnly(2026, 6, 11), new TimeOnly(13, 0, 0) },
                    { 6, new TimeOnly(20, 0, 0), 10, new DateOnly(2026, 6, 11), new TimeOnly(17, 0, 0) },
                    { 7, new TimeOnly(12, 0, 0), 10, new DateOnly(2026, 6, 12), new TimeOnly(9, 0, 0) },
                    { 8, new TimeOnly(16, 0, 0), 10, new DateOnly(2026, 6, 12), new TimeOnly(13, 0, 0) },
                    { 9, new TimeOnly(20, 0, 0), 10, new DateOnly(2026, 6, 12), new TimeOnly(17, 0, 0) },
                    { 10, new TimeOnly(12, 0, 0), 10, new DateOnly(2026, 6, 13), new TimeOnly(9, 0, 0) },
                    { 11, new TimeOnly(16, 0, 0), 10, new DateOnly(2026, 6, 13), new TimeOnly(13, 0, 0) },
                    { 12, new TimeOnly(20, 0, 0), 10, new DateOnly(2026, 6, 13), new TimeOnly(17, 0, 0) },
                    { 13, new TimeOnly(12, 0, 0), 10, new DateOnly(2026, 6, 14), new TimeOnly(9, 0, 0) },
                    { 14, new TimeOnly(16, 0, 0), 10, new DateOnly(2026, 6, 14), new TimeOnly(13, 0, 0) },
                    { 15, new TimeOnly(20, 0, 0), 10, new DateOnly(2026, 6, 14), new TimeOnly(17, 0, 0) },
                    { 16, new TimeOnly(12, 0, 0), 10, new DateOnly(2026, 6, 15), new TimeOnly(9, 0, 0) },
                    { 17, new TimeOnly(16, 0, 0), 10, new DateOnly(2026, 6, 15), new TimeOnly(13, 0, 0) },
                    { 18, new TimeOnly(20, 0, 0), 10, new DateOnly(2026, 6, 15), new TimeOnly(17, 0, 0) },
                    { 19, new TimeOnly(12, 0, 0), 10, new DateOnly(2026, 6, 16), new TimeOnly(9, 0, 0) },
                    { 20, new TimeOnly(16, 0, 0), 10, new DateOnly(2026, 6, 16), new TimeOnly(13, 0, 0) },
                    { 21, new TimeOnly(20, 0, 0), 10, new DateOnly(2026, 6, 16), new TimeOnly(17, 0, 0) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 21);
        }
    }
}
