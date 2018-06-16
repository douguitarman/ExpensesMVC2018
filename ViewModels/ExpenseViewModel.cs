using ExpensesMVC2018.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpensesMVC2018.ViewModels
{
    public class ExpenseViewModel
    {
        // Add display name and validations
        public int ExpenseId { get; set; }

        public int UserId { get; set; }

        public DateTime ExpenseDate { get; set; }

        public string ExpenseType { get; set; }

        public double ExpenseAmount { get; set; }

        public string BranchCode { get; set; }

        public string VendorName { get; set; }

        public string LastFourCcNumber { get; set; }

        public string Notes { get; set; }

        public string ReceiptFileName { get; set; }

        public byte[] DisplayReceiptImage { get; set; }

        public List<Expense> GetExpenses { get; set; }
    }
}