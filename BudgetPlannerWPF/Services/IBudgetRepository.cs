using BudgetPlannerWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerWPF.Services
{
    public interface IBudgetRepository
    {
        Task AddIncomeAsync(Income income);
        Task AddExpenseAsync(Expense expense);

        Task UpdateIncomeAsync(Income income);
        Task UpdateExpenseAsync(Expense expense);

        Task<List<Income>> GetAllIncomesAsync();
        Task<List<Expense>> GetAllExpensesAsync();

        Task<Income> GetIncomeByIdAsync(int id);
        Task<Expense> GetExpenseByIdAsync(int id);

        Task DeleteIncomeAsync(int id);
        Task DeleteExpenseAsync(int id);

        Task SaveChangesAsync();
    }

}
