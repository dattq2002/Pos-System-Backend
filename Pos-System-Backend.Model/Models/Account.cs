using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Model.Models
{
    public partial class Account
    {
        public Account()
        {
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool IsUsed { get; set; }
        public Guid? StoreId { get; set; }
        public Guid? BrandId { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual Store? Store { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
