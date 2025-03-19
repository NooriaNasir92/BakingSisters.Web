using BakingSisters.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BakingSisters.Api.Data.Configurations;

public class CustomizationGroupConfiguration : IEntityTypeConfiguration<CustomizationGroup>
{
    public void Configure(EntityTypeBuilder<CustomizationGroup> builder)
    {
        builder.HasKey(cg => cg.Id);

        builder.Property(cg => cg.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cg => cg.Description)
            .HasMaxLength(500);

        builder.Property(cg => cg.IsRequired)
            .IsRequired();

        builder.Property(cg => cg.MinSelections)
            .IsRequired();

        builder.Property(cg => cg.MaxSelections)
            .IsRequired();

        builder.HasMany(cg => cg.Options)
            .WithOne(co => co.CustomizationGroup)
            .HasForeignKey(co => co.CustomizationGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(cg => cg.ApplicableProducts)
            .WithMany(p => p.AvailableCustomizations);
    }
} 