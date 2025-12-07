using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetPlannerWPF.Models
{
    public class TransactionBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public TransactionType TransactionType { get; set; }
        public int? YearlyMonth { get; set; }
    }
}
