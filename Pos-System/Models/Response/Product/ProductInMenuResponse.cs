using Pos_System.API.Enums;

namespace Pos_System.API.Models.Response.Product;

public class ProductInMenuResponse
{
	public Guid Id { get; set; }
	public string Code { get; set; }
	public string Name { get; set; }
	public double SellingPrice { get; set; }
	public double DiscountPrice { get; set; }
	public double HistoricalPrice { get; set; }
	public int DisplayOrder { get; set; }
	public List<string> ExtraCategoryCode { get; set; } = new List<string>();
	public ProductType Type { get; set; }

}