using Pos_System.API.Enums;
using Pos_System.Domain.Paginate;

namespace Pos_System.API.Payload.Response.Collections
{
    public class GetCollectionDetailResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public CollectionStatus Status { get; set; }
        public string? PicUrl { get; set; }
        public string? Description { get; set; }

        public IPaginate<ProductOfCollection> Products { get; set; }

        public GetCollectionDetailResponse(Guid id, string name, string code, CollectionStatus status, string? picUrl, string? description)
        {
            Id = id;
            Name = name;
            Code = code;
            Status = status;
            PicUrl = picUrl;
            Description = description;
            Products = null;
        }
        public GetCollectionDetailResponse(Guid id, string name, string code, CollectionStatus status, string? picUrl, string? description, IPaginate<ProductOfCollection> products)
        {
            Id = id;
            Name = name;
            Code = code;
            Status = status;
            PicUrl = picUrl;
            Description = description;
            Products = products;
        }
    }

    public class ProductOfCollection
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public string ProductCode { get; set; }
        public string? PicUrl { get; set; }
        public double SellingPrice { get; set; }

        public ProductOfCollection(Guid id, string productName, string? description, string productCode, string? picUrl, double sellingPrice)
        {
            Id = id;
            ProductName = productName;
            Description = description;
            ProductCode = productCode;
            PicUrl = picUrl;
            SellingPrice = sellingPrice;
        }
    }
}
