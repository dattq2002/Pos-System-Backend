using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Model.Models
{
    public partial class Collection
    {
        public Collection()
        {
            CollectionProducts = new HashSet<CollectionProduct>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? Description { get; set; }
        public string? PicUrl { get; set; }

        public virtual ICollection<CollectionProduct> CollectionProducts { get; set; }
    }
}
