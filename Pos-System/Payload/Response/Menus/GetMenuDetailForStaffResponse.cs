using Pos_System.API.Enums;
using Pos_System.API.Helpers;
using System.Text.Json.Serialization;

namespace Pos_System.API.Payload.Response.Menus;
public class GetMenuDetailForStaffResponse
{
    public string Code { get; set; }
    public int Priority { get; set; }
    public bool IsBaseMenu { get; set; }
    public List<DateFilter> DateFilter { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    [JsonPropertyName("products")]
    public List<ProductInMenu> ProductsInMenu { get; set; }
    [JsonPropertyName("collections")]
    public List<CategoryInMenu> CategoryInMenu { get; set; }

    public GetMenuDetailForStaffResponse (string code, int priority, bool isBaseMenu, int dateFilter, int startTime, int endTime, List<ProductInMenu> productsInMenu, List<CategoryInMenu> categoryInMenu)
    {
        Code = code;
        Priority = priority;
        IsBaseMenu = isBaseMenu;
        DateFilter = DateTimeHelper.GetDatesFromDateFilter(dateFilter);
        StartTime = DateTimeHelper.ConvertIntToTimeOnly(startTime);
        EndTime = DateTimeHelper.ConvertIntToTimeOnly(endTime);
        ProductsInMenu = productsInMenu;
        CategoryInMenu = categoryInMenu;
    }
}

public class CategoryInMenu
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public CategoryType CategoryType { get; set; }
    public int DisplayOrder { get; set; }
    public string Description { get; set; }
    public string PicUrl { get; set; }
}

public class CollectionsInMenu
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string PicUrl { get; set; }
    public string Description { get; set; }
}