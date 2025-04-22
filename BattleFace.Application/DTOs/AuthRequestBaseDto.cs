using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Application.DTOs
{
    public abstract class AuthRequestBaseDto
    {
        public string Email { get; set; } = string.Empty;        
    }
}
