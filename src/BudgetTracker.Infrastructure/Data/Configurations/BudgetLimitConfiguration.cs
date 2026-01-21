using BudgetTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Infrastructure.Data.Configurations
{
    public class BudgetLimitConfiguration : IEntityTypeConfiguration<BudgetLimit>
    {
        public void Configure(EntityTypeBuilder<BudgetLimit> builder)
        {
            builder.ToTable("BudgetLimits");
            builder.HasKey(bl => bl.Id);

            builder.Property(bl => bl.MonthYear)
                .HasMaxLength(7)
                .IsRequired();

            builder.Property(bl => bl.LimitAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasIndex(bl => new { bl.UserId, bl.CategoryId, bl.MonthYear })
                .IsUnique();

            builder.HasOne(bl => bl.User)
                .WithMany()
                .HasForeignKey(bl => bl.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(bl => bl.Category)
                .WithMany()
                .HasForeignKey(bl => bl.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}