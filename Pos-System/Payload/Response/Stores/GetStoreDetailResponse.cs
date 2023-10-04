namespace Pos_System.API.Payload.Response.Stores;

public class GetStoreDetailResponse : GetStoreResponse
{
    public string Phone { get; set; }
    public string Code { get; set; }
    public string? BrandPicUrl { get; set; }

    public GetStoreDetailResponse(Guid id, Guid brandId, string name, string shortname, string email, string address,
        string status, string phone, string code, string brandPicUrl, string wifiName, string wifiPassword) : base(id,
        brandId, name, shortname, email,
        address, status, wifiName, wifiPassword)
    {
        Phone = phone;
        Code = code;
        BrandPicUrl = brandPicUrl;
    }
}