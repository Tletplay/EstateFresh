using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstateFresh.Migrations
{
    /// <inheritdoc />
    public partial class FixDriverLocationFieldsPlacement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LocationUpdatedAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Orders");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Users",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LocationUpdatedAt",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Users",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 1,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 30));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 2,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 30));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 3,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 30));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 4,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 31));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 5,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 31));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 6,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 31));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 7,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 1));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 8,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 1));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 9,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 1));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 10,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 2));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 11,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 2));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 12,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 2));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 13,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 3));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 14,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 3));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 15,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 3));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 16,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 4));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 17,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 4));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 18,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 4));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 19,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 5));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 20,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 5));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 21,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 5));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LocationUpdatedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Users");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Orders",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LocationUpdatedAt",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Orders",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 1,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 29));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 2,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 29));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 3,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 29));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 4,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 30));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 5,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 30));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 6,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 30));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 7,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 31));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 8,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 31));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 9,
                column: "SlotDate",
                value: new DateOnly(2026, 10, 31));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 10,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 1));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 11,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 1));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 12,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 1));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 13,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 2));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 14,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 2));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 15,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 2));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 16,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 3));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 17,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 3));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 18,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 3));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 19,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 4));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 20,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 4));

            migrationBuilder.UpdateData(
                table: "DeliverySlots",
                keyColumn: "SlotID",
                keyValue: 21,
                column: "SlotDate",
                value: new DateOnly(2026, 11, 4));
        }
    }
}
