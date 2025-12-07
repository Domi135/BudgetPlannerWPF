using BudgetPlannerWPF.Models;

namespace BudgetPlannerWPF.Services
{
    public class BudgetService : IBudgetService
    {
        public decimal CalculateMonthlyIncome(IEnumerable<Income> incomes, int month, int year)
        {
            decimal total = 0;

            foreach (var inc in incomes)
            {
                switch (inc.TransactionType)
                {
                    case TransactionType.Monthly:
                        total += inc.Amount;
                        break;

                    case TransactionType.OneTime:
                        if (inc.Date.Month == month && inc.Date.Year == year)
                            total += inc.Amount;
                        break;

                    case TransactionType.Yearly:
                        if (inc.YearlyMonth == month)
                            total += inc.Amount;
                        break;
                }
            }

            return total;
        }

        public decimal CalculateMonthlyExpenses(IEnumerable<Expense> expenses, int month, int year)
        {
            decimal total = 0;

            foreach (var exp in expenses)
            {
                switch (exp.TransactionType)
                {
                    case TransactionType.Monthly:
                        total += exp.Amount;
                        break;

                    case TransactionType.OneTime:
                        if (exp.Date.Month == month && exp.Date.Year == year)
                            total += exp.Amount;
                        break;

                    case TransactionType.Yearly:
                        if (exp.YearlyMonth == month)
                            total += exp.Amount;
                        break;
                }
            }

            return total;
        }

        public decimal CalculateMonthlyNet(
            IEnumerable<Income> incomes,
            IEnumerable<Expense> expenses,
            int month,
            int year)
        {
            return CalculateMonthlyIncome(incomes, month, year)
                 - CalculateMonthlyExpenses(expenses, month, year);
        }

        public List<Income> GetIncomesForMonth(IEnumerable<Income> incomes, int month, int year)
        {
            return incomes.Where(i =>
                i.TransactionType == TransactionType.Monthly ||
                (i.TransactionType == TransactionType.OneTime && i.Date.Month == month && i.Date.Year == year) ||
                (i.TransactionType == TransactionType.Yearly && i.YearlyMonth == month)
            ).ToList();
        }

        public List<Expense> GetExpensesForMonth(IEnumerable<Expense> expenses, int month, int year)
        {
            return expenses.Where(e =>
                e.TransactionType == TransactionType.Monthly ||
                (e.TransactionType == TransactionType.OneTime && e.Date.Month == month && e.Date.Year == year) ||
                (e.TransactionType == TransactionType.Yearly && e.YearlyMonth == month)
            ).ToList();
        }

        public decimal CalculateYearlyTotalIncome(IEnumerable<Income> incomes, int year)
        {
            decimal total = 0;

            foreach (var inc in incomes)
            {
                switch (inc.TransactionType)
                {
                    case TransactionType.Monthly:
                        total += inc.Amount * 12;
                        break;

                    case TransactionType.Yearly:
                        total += inc.Amount;
                        break;

                    case TransactionType.OneTime:
                        if (inc.Date.Year == year)
                            total += inc.Amount;
                        break;
                }
            }

            return total;
        }

        public decimal CalculateYearlyTotalExpenses(IEnumerable<Expense> expenses, int year)
        {
            decimal total = 0;

            foreach (var exp in expenses)
            {
                switch (exp.TransactionType)
                {
                    case TransactionType.Monthly:
                        total += exp.Amount * 12;
                        break;

                    case TransactionType.Yearly:
                        total += exp.Amount;
                        break;

                    case TransactionType.OneTime:
                        if (exp.Date.Year == year)
                            total += exp.Amount;
                        break;
                }
            }

            return total;
        }
    }

}
