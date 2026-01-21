using BudgetTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetTracker.Infrastructure.Data.Configurations
{
    public class RecurringTransactionConfiguration : IEntityTypeConfiguration<RecurringTransaction>
    {
        public void Configure(EntityTypeBuilder<RecurringTransaction> builder)
        {
            builder.ToTable("RecurringTransactions");
            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(rt => rt.Description)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(rt => rt.Type)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(rt => rt.Frequency)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(rt => rt.StartDate)
                .IsRequired();

            builder.Property(rt => rt.EndDate)
                .IsRequired(false); 

            builder.Property(rt => rt.IsActive)
                .HasDefaultValue(true);

            builder.Property(rt => rt.LastProcessedMonth)
                .HasMaxLength(7)
                .IsRequired(false);

            builder.HasIndex(rt => new { rt.UserId, rt.IsActive });

            builder.HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rt => rt.Category)
                .WithMany()
                .HasForeignKey(rt => rt.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
