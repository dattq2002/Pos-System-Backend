﻿using System;
using System.Collections.Generic;

namespace Pos_System.Domain.Models
{
    public partial class MenuStore
    {
        public Guid Id { get; set; }
        public Guid MenuId { get; set; }
        public Guid StoreId { get; set; }
        public int? Priority { get; set; }
        public int? DateFilter { get; set; }
        public int? StartTime { get; set; }
        public string Status { get; set; } = null!;
        public int? EndTime { get; set; }

        public virtual Menu Menu { get; set; } = null!;
        public virtual Store Store { get; set; } = null!;
    }
}
