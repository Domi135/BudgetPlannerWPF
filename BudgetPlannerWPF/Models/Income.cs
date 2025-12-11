using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerWPF.Models
{
    public class Income : TransactionBase
    {
        public IncomeCategory Category { get; set; }
        public bool IsYearlyIncome { get; set; }
        public decimal HourlyRate { get; set;}
        public int YearlyHoursWorked { get; set;}
        public decimal YearlyIncome {get; set;}
    }
}
