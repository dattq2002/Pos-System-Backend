using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Domain.Models
{
    public partial class Table
    {
        public Table()
        {
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public int Number { get; set; }
        public string Code { get; set; } = null!;
        public bool IsUsed { get; set; }
        public Guid AreaId { get; set; }

        public virtual Area Area { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
    }
}
