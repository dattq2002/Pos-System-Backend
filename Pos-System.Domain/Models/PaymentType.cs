﻿using System;
using System.Collections.Generic;

namespace Pos_System.Domain.Models
{
    public partial class PaymentType
    {
        public PaymentType()
        {
            Payments = new HashSet<Payment>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? PicUrl { get; set; }
        public bool IsDisplay { get; set; }
        public int? Position { get; set; }
        public Guid BrandId { get; set; }

        public virtual Brand Brand { get; set; } = null!;
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
