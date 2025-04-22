using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Domain.Entities
{
    public class Credential
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string PasswordHash { get; set; } = string.Empty;        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
    }
}
