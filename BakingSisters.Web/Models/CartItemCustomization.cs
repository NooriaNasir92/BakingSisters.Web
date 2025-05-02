using BakingSisters.Api.Models;

namespace BakingSisters.Web.Models;

public class CartItemCustomization
{
    public int CustomizationGroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public int CustomizationOptionId { get; set; }
    public string OptionName { get; set; } = string.Empty;
    public decimal AdditionalPrice { get; set; }
    
    // For mapping from API CustomizationOption
    public static CartItemCustomization FromOption(CustomizationOption option, CustomizationGroup group)
    {
        return new CartItemCustomization
        {
            CustomizationGroupId = group.Id,
            GroupName = group.Name,
            CustomizationOptionId = option.Id,
            OptionName = option.Name,
            AdditionalPrice = option.AdditionalPrice
        };
    }
} 