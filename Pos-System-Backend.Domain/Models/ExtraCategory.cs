using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Domain.Models
{
    public partial class ExtraCategory
    {
        public Guid Id { get; set; }
        public string ExtraCategoryCode { get; set; } = null!;
        public string ProductCategoryCode { get; set; } = null!;
        public string Status { get; set; } = null!;

        public virtual Category ExtraCategoryCodeNavigation { get; set; } = null!;
        public virtual Category ProductCategoryCodeNavigation { get; set; } = null!;
    }
}
