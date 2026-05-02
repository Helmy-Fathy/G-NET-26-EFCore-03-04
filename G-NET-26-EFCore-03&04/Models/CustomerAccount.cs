using G_NET_26_EFCore_03_04.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_26_EFCore_03_04.Models
{
    public class CustomerAccount
    {
        public Account Account { get; set; }
        public string AccountNumber { get; set; } = default!;

        public Customer Customer { get; set; }
        public int CustomerId { get; set; }

        public DateTime OwnerShipStartDate { get; set; }
        public OwnershipType OwnershipType { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }
}
