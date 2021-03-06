﻿using AuthService.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthService.EfCorePostgresProvider
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AccountEntity> Accounts => Set<AccountEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountEntity>()
                .ToTable("accounts")
                .HasKey(x => x.Login);

            modelBuilder.Entity<AccountEntity>()
                .Property(e => e.Role)
                .HasConversion<string>();
        }
    }
}
