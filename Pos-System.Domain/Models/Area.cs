using System;
using System.Collections.Generic;

namespace Pos_System.Domain.Models
{
	public partial class Area
	{
		public Area()
		{
			Pos = new HashSet<Pos>();
			Tables = new HashSet<Table>();
		}

		public Guid Id { get; set; }
		public string? Name { get; set; }
		public string? Code { get; set; }
		public int NumberOfTables { get; set; }
		public Guid StoreId { get; set; }

		public virtual Store Store { get; set; } = null!;
		public virtual ICollection<Pos> Pos { get; set; }
		public virtual ICollection<Table> Tables { get; set; }
	}
}
