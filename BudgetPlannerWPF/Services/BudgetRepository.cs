using BudgetPlannerWPF.Data;
using BudgetPlannerWPF.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlannerWPF.Services
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly BudgetContext _context;

        public BudgetRepository(BudgetContext context)
        {
            _context = context;
        }

        public async Task AddIncomeAsync(Income income)
        {
            await _context.Incomes.AddAsync(income);
        }

        public async Task AddExpenseAsync(Expense expense)
        {
            await _context.Expenses.AddAsync(expense);
        }

        public async Task UpdateIncomeAsync(Income income)
        {
            _context.Incomes.Update(income);
            await Task.CompletedTask;
        }

        public async Task UpdateExpenseAsync(Expense expense)
        {
            _context.Expenses.Update(expense);
            await Task.CompletedTask;
        }

        public async Task<List<Income>> GetAllIncomesAsync()
        {
            return await _context.Incomes.ToListAsync();
        }

        public async Task<List<Expense>> GetAllExpensesAsync()
        {
            return await _context.Expenses.ToListAsync();
        }

        public async Task<Income> GetIncomeByIdAsync(int id)
        {
            return await _context.Incomes.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Expense> GetExpenseByIdAsync(int id)
        {
            return await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task DeleteIncomeAsync(int id)
        {
            var income = await GetIncomeByIdAsync(id);
            if (income != null)
                _context.Incomes.Remove(income);
        }

        public async Task DeleteExpenseAsync(int id)
        {
            var expense = await GetExpenseByIdAsync(id);
            if (expense != null)
                _context.Expenses.Remove(expense);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
