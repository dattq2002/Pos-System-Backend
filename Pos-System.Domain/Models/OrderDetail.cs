using System;
using System.Collections.Generic;

namespace Pos_System.Domain.Models
{
    public partial class OrderDetail
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public double TotalAmount { get; set; }
        public double FinalAmount { get; set; }
        public string? Notes { get; set; }
        public Guid MenuProductId { get; set; }
        public double SellingPrice { get; set; }

        public virtual MenuProduct MenuProduct { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
