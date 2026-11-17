using Deposita.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Deposita.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");
        builder.HasKey(t => t.TransactionId);

        builder.Property(t => t.Amount).HasPrecision(18, 2).IsRequired();
        builder.Property(t => t.Description).HasMaxLength(255).IsRequired();

        builder.Property(t => t.TransactionType)
               .HasConversion<string>()
               .HasMaxLength(50)
               .IsRequired();

        builder.Property(t => t.DateCreated)
               .HasColumnType("datetime")
               .HasDefaultValueSql("GETDATE()")
               .ValueGeneratedOnAdd();
    }
}