using Pos_System.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace Pos_System.API.Payload.Request.Stores
{
    public class CreateNewStoreRequest
    {
        [Required(ErrorMessage = "Store name is required")]
        [MaxLength(50, ErrorMessage = "Store's name max length is 50 characters")]
        public string Name { get; set; }
        public string? ShortName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Code { get; set; }
        [Required(ErrorMessage = "Brand id is required")]
        public Guid BrandId { get; set; }
        public string? Address { get; set; }
    }
}
