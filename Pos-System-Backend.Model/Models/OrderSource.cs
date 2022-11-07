using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Model.Models
{
    public partial class OrderSource
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Url { get; set; }
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? PicUrl { get; set; }

        public virtual Order? Order { get; set; }
    }
}
