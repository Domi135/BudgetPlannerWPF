using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerWPF.Models
{
    public class RecurringExpense: Expense
    {
        public RecurrenceInterval Interval { get; set; }
    }
}
