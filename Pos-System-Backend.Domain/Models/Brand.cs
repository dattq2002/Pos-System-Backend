using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Domain.Models
{
    public partial class Brand
    {
        public Brand()
        {
            BrandAccounts = new HashSet<BrandAccount>();
            Products = new HashSet<Product>();
            Stores = new HashSet<Store>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        public virtual ICollection<BrandAccount> BrandAccounts { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Store> Stores { get; set; }
    }
}
