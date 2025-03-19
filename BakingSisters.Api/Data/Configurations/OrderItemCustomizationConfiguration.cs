using BakingSisters.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BakingSisters.Api.Data.Configurations;

public class OrderItemCustomizationConfiguration : IEntityTypeConfiguration<OrderItemCustomization>
{
    public void Configure(EntityTypeBuilder<OrderItemCustomization> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CustomValue)
            .HasMaxLength(255);

        builder.HasOne(c => c.OrderItem)
            .WithMany(oi => oi.Customizations)
            .HasForeignKey(c => c.OrderItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.CustomizationOption)
            .WithMany()
            .HasForeignKey(c => c.CustomizationOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 