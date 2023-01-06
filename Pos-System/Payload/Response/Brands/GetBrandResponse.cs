using Pos_System.API.Enums;

namespace Pos_System.API.Payload.Response.Brands;

public class GetBrandResponse
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string? Email { get; set; }
	public string? Address { get; set; }
	public string? Phone { get; set; }
	public string? PicUrl { get; set; }
	public BrandStatus Status { get; set; }

	public GetBrandResponse(Guid id, string name, string email, string address, string phone, string picUrl, BrandStatus status)
	{
		Id = id;
		Name = name;
		Email = email;
		Address = address;
		Phone = phone;
		PicUrl = picUrl;
		Status = status;
	}
}