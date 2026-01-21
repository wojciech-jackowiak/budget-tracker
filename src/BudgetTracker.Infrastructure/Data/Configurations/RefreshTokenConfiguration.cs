using BudgetTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Infrastructure.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(rt => rt.ExpiresAt)
                .IsRequired();

            builder.Property(rt => rt.RevokedAt)
                .IsRequired(false);

            builder.Property(rt => rt.ReplacedByToken)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(rt => rt.CreatedByIp)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(rt => rt.RevokedByIp)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.HasIndex(rt => rt.Token)
                .IsUnique();

            builder.HasIndex(rt => new { rt.UserId, rt.ExpiresAt });

            builder.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
