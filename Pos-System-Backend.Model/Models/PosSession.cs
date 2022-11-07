using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Model.Models
{
    public partial class PosSession
    {
        public Guid Id { get; set; }
        public Guid? SessionId { get; set; }
        public Guid? PosId { get; set; }

        public virtual Pos? Pos { get; set; }
        public virtual Session? Session { get; set; }
    }
}
