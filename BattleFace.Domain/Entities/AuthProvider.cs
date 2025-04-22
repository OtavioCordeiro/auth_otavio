using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Domain.Entities
{
    public class AuthProvider
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // ex: "Google", "Facebook", etc.

        public ICollection<ExternalAuthProvider> ExternalAuthLinks { get; set; } = new List<ExternalAuthProvider>();
    }
}
