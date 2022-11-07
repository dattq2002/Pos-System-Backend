using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Model.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Accounts = new HashSet<Account>();
            Products = new HashSet<Product>();
            Stores = new HashSet<Store>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
}
