using BattleFace.Domain.Entities;
using BattleFace.Domain.Interfaces;
using BattleFace.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleFace.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BattleFaceDbContext _context;

        public UserRepository(BattleFaceDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Recarrega o plano
            await _context.Entry(user).Reference(u => u.Plan).LoadAsync();

            // Recarrega os papéis (roles) do usuário
            await _context.Entry(user)
                .Collection(u => u.UserRoles)
                .Query()
                .Include(ur => ur.Role)
                .LoadAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Credential)
                .Include(u => u.Plan)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)  // Carrega as roles associadas ao usuário
                .Include(u => u.ExternalAuthProviders)  // Carrega os provedores externos associados ao usuário
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task AddPasswordRecoveryAsync(PasswordRecovery recovery)
        {
            await _context.PasswordRecoveries.AddAsync(recovery);
            await _context.SaveChangesAsync();
        }

        public async Task<PasswordRecovery?> GetValidPasswordRecoveryAsync(string token)
        {
            var now = DateTime.UtcNow;

            return await _context.PasswordRecoveries
                .Include(r => r.User)
                    .ThenInclude(u => u.Credential)
                .FirstOrDefaultAsync(r =>
                    r.Token == token &&
                    !r.Used &&
                    r.ExpiresAt > now);
        }

        public async Task MarkPasswordRecoveryAsUsedAsync(long recoveryId)
        {
            var recovery = await _context.PasswordRecoveries.FindAsync(recoveryId);

            if (recovery != null)
            {
                recovery.Used = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
