using System;
using Pos_System.API.Enums;
using Pos_System.API.Utils;
using Pos_System.Domain.Models;

namespace Pos_System.API.Payload.Response.Products
{
    public class GetProductResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? PicUrl { get; set; }
        public ProductStatus Status { get; set; }
        public ProductType Type { get; set; }
        public GetProductResponse(Guid id, string code, string name, string picUrl, string status, string type)
        {
            Id = id;
            Code = code;
            Name = name;
            PicUrl = picUrl;
            Status = EnumUtil.ParseEnum<ProductStatus>(status);
            Type = EnumUtil.ParseEnum<ProductType>(type);
        }
    }
}

