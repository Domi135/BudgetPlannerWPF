using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BudgetPlannerWPF.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Interval = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    YearlyMonth = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    IsYearlyIncome = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    YearlyMonth = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Expenses",
                columns: new[] { "Id", "Amount", "Category", "Date", "Discriminator", "Title", "TransactionType", "YearlyMonth" },
                values: new object[,]
                {
                    { 5, 650m, 0, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Expense", "Matinköp Coop", 0, null },
                    { 6, 8200m, 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Expense", "Hyra", 1, null },
                    { 7, 940m, 2, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Expense", "Busskort", 1, null },
                    { 8, 540m, 4, new DateTime(2025, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Expense", "Vaccination hund", 0, null }
                });

            migrationBuilder.InsertData(
                table: "Expenses",
                columns: new[] { "Id", "Amount", "Category", "Date", "Discriminator", "Interval", "Title", "TransactionType", "YearlyMonth" },
                values: new object[,]
                {
                    { 9, 129m, 6, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "RecurringExpense", 1, "Netflix", 1, null },
                    { 10, 4200m, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "RecurringExpense", 2, "Bilförsäkring", 2, 1 },
                    { 11, 299m, 5, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "RecurringExpense", 1, "Gymkort", 1, null },
                    { 12, 189m, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "RecurringExpense", 1, "Barnförsäkring", 1, null }
                });

            migrationBuilder.InsertData(
                table: "Incomes",
                columns: new[] { "Id", "Amount", "Category", "Date", "IsYearlyIncome", "Title", "TransactionType", "YearlyMonth" },
                values: new object[,]
                {
                    { 1, 32000m, 0, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Månadslön", 1, null },
                    { 2, 1200m, 2, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Hobbyförsäljning", 0, null },
                    { 3, 6000m, 1, new DateTime(2024, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Julbonus", 2, 12 },
                    { 4, 800m, 3, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Födelsedagspresent", 0, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Incomes");
        }
    }
}
