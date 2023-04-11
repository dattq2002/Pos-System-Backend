namespace Pos_System.API.Payload.Request.Products
{
    public class UpdateProductStatusRequest
    {
        public string Op { get; set; }
        public string Path { get; set; }
        public string Value { get; set; }
    }
}
