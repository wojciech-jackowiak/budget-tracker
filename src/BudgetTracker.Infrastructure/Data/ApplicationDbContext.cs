using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Domain.Common;
using BudgetTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IBudgetTrackerDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users{ get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<BudgetLimit> BudgetLimits { get; set; }

        public DbSet<RecurringTransaction> RecurringTransactions { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var entries = ChangeTracker.Entries<BaseEntity>()
                            .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                entry.Entity.MarkAsModified(); 
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }    
}