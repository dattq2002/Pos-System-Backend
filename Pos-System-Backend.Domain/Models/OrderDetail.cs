using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Domain.Models
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
        public int? Status { get; set; }
        public string ProductCode { get; set; } = null!;

        public virtual Order Order { get; set; } = null!;
        public virtual Product ProductCodeNavigation { get; set; } = null!;
    }
}
