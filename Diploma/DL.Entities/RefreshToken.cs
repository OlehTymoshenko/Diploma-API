using System;
using DL.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
