using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Domain.Entities
{
    public class ExternalAuthProvider
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int AuthProviderId { get; set; }
        public string ProviderUserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public AuthProvider AuthProvider { get; set; } = null!;
    }
}
