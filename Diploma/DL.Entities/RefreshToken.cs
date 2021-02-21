using System;
using System.Collections.Generic;
using System.Text;
using DL.Entities.Base;

namespace DL.Entities
{
    public class RefreshToken : BaseEntity 
    {
        public long UserId { get; set; }

        public string Token { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiredAt { get; set; }

        public User User { get; set; }
    }
}
