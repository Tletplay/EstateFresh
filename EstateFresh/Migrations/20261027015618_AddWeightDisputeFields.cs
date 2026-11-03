using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstateFresh.Migrations
{
    /// <inheritdoc />
    public partial class AddWeightDisputeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisputeReason",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisputeResolutionNote",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 1,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 27));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 2,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 27));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 3,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 27));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 4,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 28));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 5,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 28));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 6,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 28));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 7,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 29));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 8,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 29));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 9,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 29));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 10,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 30));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 11,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 30));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 12,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 30));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 13,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 31));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 14,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 31));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 15,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 31));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 16,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 1));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 17,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 1));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 18,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 1));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 19,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 2));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 20,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 2));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 21,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 2));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisputeReason",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DisputeResolutionNote",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 1,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 16));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 2,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 16));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 3,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 16));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 4,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 17));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 5,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 17));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 6,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 17));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 7,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 18));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 8,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 18));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 9,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 18));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 10,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 19));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 11,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 19));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 12,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 19));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 13,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 20));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 14,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 20));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 15,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 20));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 16,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 21));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 17,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 21));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 18,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 21));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 19,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 22));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 20,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 22));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 21,
                column: "SlotDate",
                value: new DateOnly(2026, 6, 22));
        }
    }
}
