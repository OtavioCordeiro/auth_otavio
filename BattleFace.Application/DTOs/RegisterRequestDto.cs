using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Application.DTOs
{
    public class RegisterRequestDto : AuthRequestBaseDto
    {
        public string Name { get; set; } = string.Empty;        
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
