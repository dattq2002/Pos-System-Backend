using System;
using System.Collections.Generic;

namespace Pos_System.Domain.Models
{
    public partial class Product
    {
        public Product()
        {
            CollectionProducts = new HashSet<CollectionProduct>();
            InverseParentProduct = new HashSet<Product>();
            MenuProducts = new HashSet<MenuProduct>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public double SellingPrice { get; set; }
        public string? PicUrl { get; set; }
        public string Status { get; set; } = null!;
        public double HistoricalPrice { get; set; }
        public double DiscountPrice { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public string? Size { get; set; }
        public string Type { get; set; } = null!;
        public Guid? ParentProductId { get; set; }
        public Guid BrandId { get; set; }
        public Guid CategoryId { get; set; }

        public virtual Brand Brand { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        public virtual Product? ParentProduct { get; set; }
        public virtual ICollection<CollectionProduct> CollectionProducts { get; set; }
        public virtual ICollection<Product> InverseParentProduct { get; set; }
        public virtual ICollection<MenuProduct> MenuProducts { get; set; }
    }
}
