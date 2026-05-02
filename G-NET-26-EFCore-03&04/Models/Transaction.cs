using G_NET_26_EFCore_03_04.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_26_EFCore_03_04.Models
{
    public class Transaction
    {
        public long TransactionNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }
        public TransactionType TransactionType { get; set; }

        public Account Account { get; set; } = default!;
        public string AccountNumber { get; set; }
    }
}
