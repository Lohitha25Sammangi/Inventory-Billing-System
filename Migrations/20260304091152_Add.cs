using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryBilling.API.Migrations
{
    /// <inheritdoc />
    public partial class Add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentMode",
                table: "Payments",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "AmountPaid",
                table: "Payments",
                newName: "Amount");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "Invoices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidAt",
                table: "Invoices",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "PaidAt",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Payments",
                newName: "PaymentMode");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Payments",
                newName: "AmountPaid");
        }
    }
}
