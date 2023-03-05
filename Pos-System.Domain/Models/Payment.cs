using System;
using System.Collections.Generic;

namespace Pos_System.Domain.Models
{
    public partial class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public double Amount { get; set; }
        public Guid PaymentTypeId { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual PaymentType PaymentType { get; set; } = null!;
    }
}
