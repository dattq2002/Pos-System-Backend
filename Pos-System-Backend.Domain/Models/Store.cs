using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Domain.Models
{
    public partial class Store
    {
        public Store()
        {
            Areas = new HashSet<Area>();
            MenuStores = new HashSet<MenuStore>();
            StoreAccounts = new HashSet<StoreAccount>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Status { get; set; } = null!;
        public Guid BrandId { get; set; }

        public virtual Brand Brand { get; set; } = null!;
        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<MenuStore> MenuStores { get; set; }
        public virtual ICollection<StoreAccount> StoreAccounts { get; set; }
    }
}
