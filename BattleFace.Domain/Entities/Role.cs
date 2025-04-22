using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "admin", "user"

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
