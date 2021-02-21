using System;
using System.Collections.Generic;
using System.Text;
using DL.Entities.Base;

namespace DL.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
