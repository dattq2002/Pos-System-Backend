using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Domain.Models
{
    public partial class Pos
    {
        public Pos()
        {
            DateReports = new HashSet<DateReport>();
            PosSessions = new HashSet<PosSession>();
        }

        public Guid Id { get; set; }
        public string? Location { get; set; }
        public string? Address { get; set; }
        public double? CashboxAmount { get; set; }
        public Guid AreaId { get; set; }

        public virtual Area Area { get; set; } = null!;
        public virtual ICollection<DateReport> DateReports { get; set; }
        public virtual ICollection<PosSession> PosSessions { get; set; }
    }
}
