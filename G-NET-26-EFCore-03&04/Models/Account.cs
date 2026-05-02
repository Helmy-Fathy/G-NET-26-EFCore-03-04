using G_NET_26_EFCore_03_04.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_26_EFCore_03_04.Models
{
    public class Account
    {
        public string AccountNumber { get; set; } = default!;
        public decimal CurrentBalance { get; set; }
        public AccountType  AccountType { get; set; }
        public DateTime OpeningDate { get; set; }

        public Branch Branch { get; set; } = default!;
        public string BranchCode { get; set; } = default!;

        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new HashSet<CustomerAccount>();

        public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();

    }
}
