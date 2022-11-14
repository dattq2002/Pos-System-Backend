using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Domain.Models
{
    public partial class Category
    {
        public Category()
        {
            ExtraCategoryExtraCategoryCodeNavigations = new HashSet<ExtraCategory>();
            ExtraCategoryProductCategoryCodeNavigations = new HashSet<ExtraCategory>();
            Products = new HashSet<Product>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public string? Description { get; set; }
        public string? PicUrl { get; set; }
        public string Status { get; set; } = null!;

        public virtual ICollection<ExtraCategory> ExtraCategoryExtraCategoryCodeNavigations { get; set; }
        public virtual ICollection<ExtraCategory> ExtraCategoryProductCategoryCodeNavigations { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
