using Pos_System.API.Models.Response.Product;

namespace Pos_System.API.Models.Response.Menu;

public class MenuResponse
{
	public Guid Id { get; set; }
	public string Code { get; set; }
	public List<ProductInMenuResponse> Products { get; set; } = new List<ProductInMenuResponse>();
	//public List<ProductInMenuResponse> ProductsExtra { get; set; } = new List<ProductInMenuResponse>();
}