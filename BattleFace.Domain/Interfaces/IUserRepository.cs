using BattleFace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task AddPasswordRecoveryAsync(PasswordRecovery recovery);
        Task<PasswordRecovery?> GetValidPasswordRecoveryAsync(string token);
        Task MarkPasswordRecoveryAsUsedAsync(long recoveryId);
    }
}
