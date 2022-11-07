using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Model.Models
{
    public partial class CollectionProduct
    {
        public Guid Id { get; set; }
        public string CollectionCode { get; set; } = null!;
        public string ProductCode { get; set; } = null!;
        public string Status { get; set; } = null!;

        public virtual Collection CollectionCodeNavigation { get; set; } = null!;
        public virtual Product ProductCodeNavigation { get; set; } = null!;
    }
}
