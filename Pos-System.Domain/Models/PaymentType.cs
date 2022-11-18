using System;
using System.Collections.Generic;

namespace Pos_System.Domain.Models
{
    public partial class PaymentType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Icon { get; set; }
        public bool IsDisplay { get; set; }
        public int? Position { get; set; }

        public virtual Payment? Payment { get; set; }
    }
}
