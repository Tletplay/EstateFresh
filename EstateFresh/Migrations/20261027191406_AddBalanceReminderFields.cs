using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstateFresh.Migrations
{
    /// <inheritdoc />
    public partial class AddBalanceReminderFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BalanceReminderCount",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastReminderSentAt",
                table: "Orders",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BalanceReminderCount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "LastReminderSentAt",
                table: "Orders");
        }
    }
}
