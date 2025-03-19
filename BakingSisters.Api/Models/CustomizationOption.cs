using BakingSisters.Api.Models.Enum;

namespace BakingSisters.Api.Models;

public class CustomizationOption : BaseModel
{
    public int CustomizationGroupId { get; set; }
    public virtual CustomizationGroup CustomizationGroup { get; set; } = new();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal AdditionalPrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public CustomizationOptionType OptionType { get; set; }
}