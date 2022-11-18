using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Domain.Models
{
    public partial class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public double Amount { get; set; }
        public Guid PaymentTypeId { get; set; }

        public virtual PaymentType IdNavigation { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
