using BudgetPlannerWPF.Commands;
using BudgetPlannerWPF.Models;
using BudgetPlannerWPF.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace BudgetPlannerWPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IBudgetRepository _repository;
        private readonly IBudgetService _budgetService;

        public Array TransactionTypes => Enum.GetValues(typeof(TransactionType));
        public Array IncomeCategories => Enum.GetValues(typeof(IncomeCategory));
        public Array ExpenseCategories => Enum.GetValues(typeof(ExpenseCategory));


        public ObservableCollection<Income> Incomes { get; } = new();
        public ObservableCollection<Expense> Expenses { get; } = new();

        // ------------------------------------------------------------
        // Selected items in lists
        // ------------------------------------------------------------
        private Income _selectedIncome;
        public Income SelectedIncome
        {
            get => _selectedIncome;
            set { _selectedIncome = value; RaisePropertyChanged(); }
        }

        private Expense _selectedExpense;
        public Expense SelectedExpense
        {
            get => _selectedExpense;
            set { _selectedExpense = value; RaisePropertyChanged(); }
        }

        // ------------------------------------------------------------
        // FORM VISIBILITY + EDITING MODELS
        // ------------------------------------------------------------
        private bool _isFormVisible;
        public bool IsFormVisible
        {
            get => _isFormVisible;
            set { _isFormVisible = value; RaisePropertyChanged(); }
        }

        private Income _editingIncome;
        public Income EditingIncome
        {
            get => _editingIncome;
            set { _editingIncome = value; RaisePropertyChanged(); }
        }

        private Expense _editingExpense;
        public Expense EditingExpense
        {
            get => _editingExpense;
            set { _editingExpense = value; RaisePropertyChanged(); }
        }

        // ------------------------------------------------------------
        // Summary
        // ------------------------------------------------------------
        public int SelectedMonth { get; set; } = DateTime.Now.Month;
        public int SelectedYear { get; set; } = DateTime.Now.Year;

        private decimal _monthlyIncome;
        public decimal MonthlyIncome
        {
            get => _monthlyIncome;
            set { _monthlyIncome = value; RaisePropertyChanged(); }
        }

        private decimal _monthlyExpenses;
        public decimal MonthlyExpenses
        {
            get => _monthlyExpenses;
            set { _monthlyExpenses = value; RaisePropertyChanged(); }
        }

        private decimal _monthlyNet;
        public decimal MonthlyNet
        {
            get => _monthlyNet;
            set { _monthlyNet = value; RaisePropertyChanged(); }
        }

        // ------------------------------------------------------------
        // Commands
        // ------------------------------------------------------------
        public DelegateCommand AddIncomeCommand { get; }
        public DelegateCommand EditIncomeCommand { get; }
        public DelegateCommand DeleteIncomeCommand { get; }

        public DelegateCommand AddExpenseCommand { get; }
        public DelegateCommand EditExpenseCommand { get; }
        public DelegateCommand DeleteExpenseCommand { get; }

        public DelegateCommand SaveFormCommand { get; }
        public DelegateCommand CancelFormCommand { get; }

        // ------------------------------------------------------------
        // Constructor
        // ------------------------------------------------------------
        public MainViewModel(IBudgetRepository repo, IBudgetService service)
        {
            _repository = repo;
            _budgetService = service;

            AddIncomeCommand = new DelegateCommand(_ => StartAddIncome());
            EditIncomeCommand = new DelegateCommand(_ => StartEditIncome(), _ => SelectedIncome != null);
            DeleteIncomeCommand = new DelegateCommand(_ => DeleteIncome(), _ => SelectedIncome != null);

            AddExpenseCommand = new DelegateCommand(_ => StartAddExpense());
            EditExpenseCommand = new DelegateCommand(_ => StartEditExpense(), _ => SelectedExpense != null);
            DeleteExpenseCommand = new DelegateCommand(_ => DeleteExpense(), _ => SelectedExpense != null);

            SaveFormCommand = new DelegateCommand(_ => SaveForm());
            CancelFormCommand = new DelegateCommand(_ => CancelForm());

            LoadDataAsync();
        }

        // ------------------------------------------------------------
        // Load from DB
        // ------------------------------------------------------------
        private async Task LoadDataAsync()
        {
            var incomes = await _repository.GetAllIncomesAsync();
            var expenses = await _repository.GetAllExpensesAsync();

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Incomes.Clear();
                Expenses.Clear();

                foreach (var i in incomes) Incomes.Add(i);
                foreach (var e in expenses) Expenses.Add(e);

                RefreshTotals();
            });
        }

        // ------------------------------------------------------------
        // ADD / EDIT — open form
        // ------------------------------------------------------------
        private void StartAddIncome()
        {
            EditingExpense = null;
            EditingIncome = new Income
            {
                Date = DateTime.Now,
                TransactionType = TransactionType.Monthly,
                Category = IncomeCategory.Salary
            };

            IsFormVisible = true;
        }

        private void StartEditIncome()
        {
            EditingExpense = null;
            EditingIncome = SelectedIncome;
            IsFormVisible = true;
        }

        private void StartAddExpense()
        {
            EditingIncome = null;
            EditingExpense = new Expense
            {
                Date = DateTime.Now,
                TransactionType = TransactionType.Monthly,
                Category = ExpenseCategory.Food
            };

            IsFormVisible = true;
        }

        private void StartEditExpense()
        {
            EditingIncome = null;
            EditingExpense = SelectedExpense;
            IsFormVisible = true;
        }

        // ------------------------------------------------------------
        // DELETE
        // ------------------------------------------------------------
        private async void DeleteIncome()
        {
            if (SelectedIncome == null) return;

            await _repository.DeleteIncomeAsync(SelectedIncome.Id);
            await _repository.SaveChangesAsync();

            Incomes.Remove(SelectedIncome);
            SelectedIncome = null;

            RefreshTotals();
        }

        private async void DeleteExpense()
        {
            if (SelectedExpense == null) return;

            await _repository.DeleteExpenseAsync(SelectedExpense.Id);
            await _repository.SaveChangesAsync();

            Expenses.Remove(SelectedExpense);
            SelectedExpense = null;

            RefreshTotals();
        }

        // ------------------------------------------------------------
        // SAVE / CANCEL FORM
        // ------------------------------------------------------------
        private async void SaveForm()
        {
            if (EditingIncome != null)
            {
                if (EditingIncome.Id == 0) await _repository.AddIncomeAsync(EditingIncome);
            }
            else if (EditingExpense != null)
            {
                if (EditingExpense.Id == 0) await _repository.AddExpenseAsync(EditingExpense);
            }

            await _repository.SaveChangesAsync();

            if (EditingIncome != null && !Incomes.Contains(EditingIncome))
                Incomes.Add(EditingIncome);

            if (EditingExpense != null && !Expenses.Contains(EditingExpense))
                Expenses.Add(EditingExpense);

            CancelForm();
            RefreshTotals();
        }

        private void CancelForm()
        {
            EditingIncome = null;
            EditingExpense = null;
            IsFormVisible = false;
        }

        // ------------------------------------------------------------
        // REFRESH TOTALS (with service)
        // ------------------------------------------------------------
        private void RefreshTotals()
        {
            MonthlyIncome =
                _budgetService.CalculateMonthlyIncome(Incomes, SelectedMonth, SelectedYear);

            MonthlyExpenses =
                _budgetService.CalculateMonthlyExpenses(Expenses, SelectedMonth, SelectedYear);

            MonthlyNet = MonthlyIncome - MonthlyExpenses;
        }
    }
}
