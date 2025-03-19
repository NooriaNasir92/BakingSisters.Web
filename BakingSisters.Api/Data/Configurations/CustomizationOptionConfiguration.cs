using BakingSisters.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BakingSisters.Api.Data.Configurations;

public class CustomizationOptionConfiguration : IEntityTypeConfiguration<CustomizationOption>
{
    public void Configure(EntityTypeBuilder<CustomizationOption> builder)
    {
        builder.HasKey(co => co.Id);

        builder.Property(co => co.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(co => co.Description)
            .HasMaxLength(500);

        builder.Property(co => co.AdditionalPrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(co => co.ImageUrl)
            .HasMaxLength(255);

        builder.Property(co => co.OptionType)
            .IsRequired();

        builder.HasOne(co => co.CustomizationGroup)
            .WithMany(cg => cg.Options)
            .HasForeignKey(co => co.CustomizationGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 