using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_26_EFCore_03_04.Models
{
    public class Branch
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;

        public int ManagerId { get; set; }
        public Manager Manager { get; set; } = default!;

        public ICollection<Account> Accounts { get; set; } = new HashSet<Account>();
    }
}
