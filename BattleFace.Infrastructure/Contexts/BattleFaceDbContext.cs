using BattleFace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleFace.Infrastructure.Contexts
{
    public class BattleFaceDbContext : DbContext
    {
        public BattleFaceDbContext(DbContextOptions<BattleFaceDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Credential> Credentials => Set<Credential>();
        public DbSet<ExternalAuthProvider> ExternalAuthProviders => Set<ExternalAuthProvider>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<PasswordRecovery> PasswordRecoveries => Set<PasswordRecovery>();
        public DbSet<Plan> Plans { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

            modelBuilder.Entity<Credential>()
                .HasOne(c => c.User)
                .WithOne(u => u.Credential)
                .HasForeignKey<Credential>(c => c.UserId);

            modelBuilder.Entity<ExternalAuthProvider>()
                .HasOne(p => p.User)
                .WithMany(u => u.ExternalAuthProviders)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<PasswordRecovery>()
                .HasOne(pr => pr.User)
                .WithMany(u => u.PasswordRecoveries)
                .HasForeignKey(pr => pr.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "User" },
                new Role { Id = 2, Name = "Moderator" },
                new Role { Id = 3, Name = "Admin" });

            modelBuilder.Entity<Plan>().HasData(
                new Plan { Id = 1, Name = "Basic", Description = "Free access", PriceInCents = 0 },
                new Plan { Id = 2, Name = "Premium", Description = "Advanced benefits", PriceInCents = 1999 });

            modelBuilder.Entity<AuthProvider>().HasData(
                new AuthProvider { Id = 1, Name = "Google" });
        }
    }
}
