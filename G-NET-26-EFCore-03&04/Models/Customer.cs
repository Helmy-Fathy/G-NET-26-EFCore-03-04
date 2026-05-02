using G_NET_26_EFCore_03_04.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_26_EFCore_03_04.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string NationalID { get; set; } = default!;
        public DateTime DateOfBirth { get; set; }
        public CustomerType CustomerType { get; set; }

        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new HashSet<CustomerAccount>();
    }
}
