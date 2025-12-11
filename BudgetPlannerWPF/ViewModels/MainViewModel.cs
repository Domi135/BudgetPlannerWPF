using BudgetPlannerWPF.Commands;
using BudgetPlannerWPF.Models;
using BudgetPlannerWPF.Services;
using System.Collections.ObjectModel;
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
        // Selected items
        // ------------------------------------------------------------
        private Income _selectedIncome;
        public Income SelectedIncome
        {
            get => _selectedIncome;
            set
            {
                _selectedIncome = value;
                RaisePropertyChanged();
                EditIncomeCommand?.RaiseCanExecuteChanged();
                DeleteIncomeCommand?.RaiseCanExecuteChanged();
            }
        }

        private Expense _selectedExpense;
        public Expense SelectedExpense
        {
            get => _selectedExpense;
            set
            {
                _selectedExpense = value;
                RaisePropertyChanged();
                EditExpenseCommand?.RaiseCanExecuteChanged();
                DeleteExpenseCommand?.RaiseCanExecuteChanged();
            }
        }

        // ------------------------------------------------------------
        // Visibility-based UI control
        // ------------------------------------------------------------
        private Visibility _formVisibility = Visibility.Collapsed;
        public Visibility FormVisibility
        {
            get => _formVisibility;
            set { _formVisibility = value; RaisePropertyChanged(); }
        }

        private Visibility _mainContentVisibility = Visibility.Visible;
        public Visibility MainContentVisibility
        {
            get => _mainContentVisibility;
            set { _mainContentVisibility = value; RaisePropertyChanged(); }
        }

        // ------------------------------------------------------------
        // Editing objects
        // ------------------------------------------------------------
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
        // Summary fields
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
        // Load
        // ------------------------------------------------------------
        private async Task LoadDataAsync()
        {
            var incomes = await _repository.GetAllIncomesAsync();
            var expenses = await _repository.GetAllExpensesAsync();

            Application.Current.Dispatcher.Invoke(() =>
            {
                Incomes.Clear();
                Expenses.Clear();

                foreach (var i in incomes) Incomes.Add(i);
                foreach (var e in expenses) Expenses.Add(e);

                RefreshTotals();
            });
        }

        // ------------------------------------------------------------
        // SHOW/HIDE FORM
        // ------------------------------------------------------------
        private void ShowForm()
        {
            MainContentVisibility = Visibility.Collapsed;
            FormVisibility = Visibility.Visible;
        }

        private void HideForm()
        {
            EditingIncome = null;
            EditingExpense = null;
            MainContentVisibility = Visibility.Visible;
            FormVisibility = Visibility.Collapsed;
        }

        // ------------------------------------------------------------
        // Add/Edit
        // ------------------------------------------------------------
        private void StartAddIncome()
        {
            EditingIncome = new Income
            {
                Date = DateTime.Now,
                Category = IncomeCategory.Salary,
                TransactionType = TransactionType.Monthly
            };

            EditingExpense = null;
            ShowForm();
        }

        private void StartEditIncome()
        {
            if (SelectedIncome == null) return;

            EditingIncome = SelectedIncome;
            EditingExpense = null;
            ShowForm();
        }

        private void StartAddExpense()
        {
            EditingExpense = new Expense
            {
                Date = DateTime.Now,
                Category = ExpenseCategory.Food,
                TransactionType = TransactionType.Monthly
            };

            EditingIncome = null;
            ShowForm();
        }

        private void StartEditExpense()
        {
            if (SelectedExpense == null) return;

            EditingExpense = SelectedExpense;
            EditingIncome = null;
            ShowForm();
        }

        // ------------------------------------------------------------
        // Delete
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
        // Save / Cancel
        // ------------------------------------------------------------
        private async void SaveForm()
        {
            if (EditingIncome != null)
            {
                if (EditingIncome.Id == 0)
                    await _repository.AddIncomeAsync(EditingIncome);
            }
            else if (EditingExpense != null)
            {
                if (EditingExpense.Id == 0)
                    await _repository.AddExpenseAsync(EditingExpense);
            }

            await _repository.SaveChangesAsync();

            if (EditingIncome != null && !Incomes.Contains(EditingIncome))
                Incomes.Add(EditingIncome);

            if (EditingExpense != null && !Expenses.Contains(EditingExpense))
                Expenses.Add(EditingExpense);

            HideForm();
            RefreshTotals();
        }

        private void CancelForm()
        {
            HideForm();
        }

        // ------------------------------------------------------------
        // Totals
        // ------------------------------------------------------------
        private void RefreshTotals()
        {
            MonthlyIncome =
                _budgetService.CalculateMonthlyIncome(Incomes, SelectedMonth, SelectedYear);

            MonthlyExpenses =
                _budgetService.CalculateMonthlyExpenses(Expenses, SelectedMonth, SelectedYear);

            MonthlyNet = MonthlyIncome - MonthlyExpenses;
        }

        //------------------------------------------------------------
        // Smmary for upcoming month
        // ------------------------------------------------------------

        // ------------------------------------------------------------
        // Monthly income based on yearly income and hours worked
        // ------------------------------------------------------------

    }
}

