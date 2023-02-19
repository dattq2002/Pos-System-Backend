namespace Pos_System.API.Payload.Request.Menus
{
    public class CreateNewMenuRequest
    {
        public string Code { get; set; }
        public List<ProductInMenu> ProductsInMenu { get; set; } = new List<ProductInMenu>();
        public List<StoreInMenu> StoresInMenu { get; set; } = new List<StoreInMenu>();
    }

    public class ProductInMenu
    {
        public Guid Id { get; set; }
        public double SellingPrice { get; set; }
        public double? DiscountPrice { get; set; }
    }

    public class StoreInMenu
    {
        public Guid Id { get; set; }
        public int? Priority { get; set; }
        public int? DateFilter { get; set; }
        public int? StartTime { get; set; }
        public int? EndTime { get; set; }
    }
}
