using BudgetPlannerWPF.Models;
using BudgetPlannerWPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BudgetPlannerWPF.ViewModels
{
    public class IncomeViewModel : ViewModelBase
    {
        private Income _income;

        public IncomeViewModel(Income income)
        {
            _income = income;
        }

        public Income Model => _income;

        public string Title
        {
            get => _income.Title;
            set { _income.Title = value; RaisePropertyChanged(); }
        }

        public decimal Amount
        {
            get => _income.Amount;
            set { _income.Amount = value; RaisePropertyChanged(); }
        }
        public DateTime Date
        {
            get => _income.Date;
            set { _income.Date = value; RaisePropertyChanged(); }
        }

        public IncomeCategory Category
        {
            get => _income.Category;
            set { _income.Category = value; RaisePropertyChanged(); }
        }

        public TransactionType TransactionType
        {
            get => _income.TransactionType;
            set { _income.TransactionType = value; RaisePropertyChanged(); }
        }
        public int? YearlyMonth
        {
            get => _income.YearlyMonth;
            set { _income.YearlyMonth = value; RaisePropertyChanged(); }
        }
        public bool IsYearlyIncome
        {
            get => _income.IsYearlyIncome;
            set { _income.IsYearlyIncome = value; RaisePropertyChanged(); }
        }
    }
}
