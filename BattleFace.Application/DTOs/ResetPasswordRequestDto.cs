﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Application.DTOs
{
    public class ResetPasswordRequestDto
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
