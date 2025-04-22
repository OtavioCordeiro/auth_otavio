using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Application.DTOs
{

    public class LoginRequestDto : AuthRequestBaseDto
    {
        public string Password { get; set; } = string.Empty;
    }
}
