using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Domain.Entities
{
    public class PasswordRecovery
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string Token { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool Used { get; set; }

        public DateTime CreatedAt { get; set; }

        public User? User { get; set; }
    }

}
