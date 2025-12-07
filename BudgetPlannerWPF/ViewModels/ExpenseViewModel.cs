using BudgetPlannerWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerWPF.ViewModels
{
    public class ExpenseViewModel : ViewModelBase
    {
        private readonly Expense _expense;

        public ExpenseViewModel(Expense expense)
        {
            _expense = expense;
        }

        public Expense Model => _expense;

        public string Title
        {
            get => _expense.Title;
            set { _expense.Title = value; RaisePropertyChanged(); }
        }

        public decimal Amount
        {
            get => _expense.Amount;
            set { _expense.Amount = value; RaisePropertyChanged(); }
        }
        public DateTime Date
        {
            get => _expense.Date;
            set { _expense.Date = value; RaisePropertyChanged(); }
        }

        public ExpenseCategory Category
        {
            get => _expense.Category;
            set { _expense.Category = value; RaisePropertyChanged(); }
        }

        public TransactionType TransactionType
        {
            get => _expense.TransactionType;
            set { _expense.TransactionType = value; RaisePropertyChanged(); }
        }
        public int? YearlyMonth
        {
            get => _expense.YearlyMonth;
            set { _expense.YearlyMonth = value; RaisePropertyChanged(); }
        }
    }
}
