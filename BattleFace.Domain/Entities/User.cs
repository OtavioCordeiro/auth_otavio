using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Domain.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int PlanId { get; set; }

        public Credential Credential { get; set; }
        public Plan Plan { get; set; }
        public ICollection<ExternalAuthProvider> ExternalAuthProviders { get; set; } = new List<ExternalAuthProvider>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<PasswordRecovery> PasswordRecoveries { get; set; } = new List<PasswordRecovery>();
    }
}
