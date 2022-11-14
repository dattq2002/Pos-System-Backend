﻿using System;
using System.Collections.Generic;

namespace Pos_System_Backend.Model.Models
{
    public partial class Role
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
