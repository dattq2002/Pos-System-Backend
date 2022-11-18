using System;
using System.Collections.Generic;

namespace Pos_System.Domain.Models
{
	public partial class DateReport
	{
		public Guid Id { get; set; }
		public DateTime Date { get; set; }
		public double FinalAmount { get; set; }
		public double FinalAmountAtStore { get; set; }
		public double FinalAmountDelivery { get; set; }
		public double FinalAmountTakeAway { get; set; }
		public double TotalAmount { get; set; }
		public double TotalCrash { get; set; }
		public int TotalOrder { get; set; }
		public int TotalCardDelivery { get; set; }
		public int TotalOrderTakeAway { get; set; }
		public Guid PosId { get; set; }

		public virtual Pos Pos { get; set; } = null!;
	}
}
