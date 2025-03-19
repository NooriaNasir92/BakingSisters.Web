using BakingSisters.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BakingSisters.Api.Data.Configurations;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Description)
            .HasMaxLength(500);

        builder.Property(i => i.UnitOfMeasure)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.ImageUrl)
            .HasMaxLength(255);
    }
} 