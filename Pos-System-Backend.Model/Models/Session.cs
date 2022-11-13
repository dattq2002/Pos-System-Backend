using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Model.Models
{
    public partial class Session
    {
        public Session()
        {
            Orders = new HashSet<Order>();
            PosSessions = new HashSet<PosSession>();
        }

        public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int NumberOfOrders { get; set; }
        public Guid AccountId { get; set; }
        public double? TotalAmount { get; set; }
        public int? TotalPromotion { get; set; }
        public double? TotalChangeCash { get; set; }
        public double? TotalDiscountAmount { get; set; }
        public double? TotalFinalAmount { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PosSession> PosSessions { get; set; }
    }
}
