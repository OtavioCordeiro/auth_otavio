﻿using BattleFace.Application.DTOs;
using BattleFace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(UserDto user);
    }
}
