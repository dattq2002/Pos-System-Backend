namespace Pos_System.API.Payload.Request.Menus
{
    public class CreateNewMenuRequest
    {
        public string Code { get; set; }
        public List<ProductOfMenu> ProductOfMenu { get; set; } = new List<ProductOfMenu>();
        public List<StoreMenuAdditionInformation> StoreMenuAdditionInformation { get; set; } = new List<StoreMenuAdditionInformation>();
    }

    public class ProductOfMenu
    {
        public Guid Id { get; set; }
        public double SellingPrice { get; set; }
        public double? DiscountPrice { get; set; }
    }

    public class StoreMenuAdditionInformation
    {
        public Guid Id { get; set; }
        public int? Priority { get; set; }
        public int? DateFilter { get; set; }
        public int? TimeFilter { get; set; }
    }
}
