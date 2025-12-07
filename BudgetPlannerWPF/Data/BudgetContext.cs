using BudgetPlannerWPF.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BudgetPlannerWPF.Data
{
    public class BudgetContext : DbContext
    {
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<RecurringExpense> RecurringExpenses { get; set; }

        public BudgetContext(DbContextOptions<BudgetContext> options) : base(options)
        {
        }

        public BudgetContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BudgetPlannerDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===============================================
            // FIX: ValueGeneratedNever för att slippa IDENTITY-problem
            // ===============================================
            modelBuilder.Entity<Income>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<Expense>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<RecurringExpense>().Property(x => x.Id).ValueGeneratedNever();

            // ============================
            //          INCOME (4)
            // ============================
            modelBuilder.Entity<Income>().HasData(
                new Income { Id = 1, Title = "Månadslön", Amount = 32000m, Date = new DateTime(2025, 1, 25), TransactionType = TransactionType.Monthly, Category = IncomeCategory.Salary, IsYearlyIncome = false },
                new Income { Id = 2, Title = "Hobbyförsäljning", Amount = 1200m, Date = new DateTime(2025, 2, 5), TransactionType = TransactionType.OneTime, Category = IncomeCategory.Hobby, IsYearlyIncome = false },
                new Income { Id = 3, Title = "Julbonus", Amount = 6000m, Date = new DateTime(2024, 12, 20), TransactionType = TransactionType.Yearly, Category = IncomeCategory.Bonus, IsYearlyIncome = true, YearlyMonth = 12 },
                new Income { Id = 4, Title = "Födelsedagspresent", Amount = 800m, Date = new DateTime(2025, 3, 10), TransactionType = TransactionType.OneTime, Category = IncomeCategory.Gift, IsYearlyIncome = false }
            );

            // ============================
            //          EXPENSE (4)
            // ============================
            modelBuilder.Entity<Expense>().HasData(
                new Expense { Id = 5, Title = "Matinköp Coop", Amount = 650m, Date = new DateTime(2025, 1, 3), TransactionType = TransactionType.OneTime, Category = ExpenseCategory.Food },
                new Expense { Id = 6, Title = "Hyra", Amount = 8200m, Date = new DateTime(2025, 1, 1), TransactionType = TransactionType.Monthly, Category = ExpenseCategory.Housing },
                new Expense { Id = 7, Title = "Busskort", Amount = 940m, Date = new DateTime(2025, 1, 10), TransactionType = TransactionType.Monthly, Category = ExpenseCategory.Transport },
                new Expense { Id = 8, Title = "Vaccination hund", Amount = 540m, Date = new DateTime(2025, 2, 22), TransactionType = TransactionType.OneTime, Category = ExpenseCategory.Animals }
            );

            // ============================
            //     RECURRING EXPENSE (4)
            // ============================
            modelBuilder.Entity<RecurringExpense>().HasData(
                new RecurringExpense { Id = 9, Title = "Netflix", Amount = 129m, Date = new DateTime(2025, 1, 1), TransactionType = TransactionType.Monthly, Interval = RecurrenceInterval.Monthly, Category = ExpenseCategory.Streaming },
                new RecurringExpense { Id = 10, Title = "Bilförsäkring", Amount = 4200m, Date = new DateTime(2025, 1, 1), TransactionType = TransactionType.Yearly, Interval = RecurrenceInterval.Yearly, Category = ExpenseCategory.Transport, YearlyMonth = 1 },
                new RecurringExpense { Id = 11, Title = "Gymkort", Amount = 299m, Date = new DateTime(2025, 1, 2), TransactionType = TransactionType.Monthly, Interval = RecurrenceInterval.Monthly, Category = ExpenseCategory.Health },
                new RecurringExpense { Id = 12, Title = "Barnförsäkring", Amount = 189m, Date = new DateTime(2025, 1, 1), TransactionType = TransactionType.Monthly, Interval = RecurrenceInterval.Monthly, Category = ExpenseCategory.Children }
            );
        }
    }
}
