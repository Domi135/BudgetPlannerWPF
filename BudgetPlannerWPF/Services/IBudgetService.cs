using BudgetPlannerWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerWPF.Services
{
    public interface IBudgetService
    {
        decimal CalculateMonthlyIncome(IEnumerable<Income> incomes, int month, int year);
        decimal CalculateMonthlyExpenses(IEnumerable<Expense> expenses, int month, int year);

        decimal CalculateMonthlyNet(IEnumerable<Income> incomes, IEnumerable<Expense> expenses, int month, int year);

        List<Income> GetIncomesForMonth(IEnumerable<Income> incomes, int month, int year);
        List<Expense> GetExpensesForMonth(IEnumerable<Expense> expenses, int month, int year);

        decimal CalculateYearlyTotalIncome(IEnumerable<Income> incomes, int year);
        decimal CalculateYearlyTotalExpenses(IEnumerable<Expense> expenses, int year);
    }
}
