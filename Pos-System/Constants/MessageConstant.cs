namespace Pos_System.API.Constants;

public static class MessageConstant
{
    public static class LoginMessage
    {
        public const string InvalidUsernameOrPassword = "Username or password is not correct";
        public const string DeactivatedAccount = "Deactivated account can not login system";
    }
    public static class Brand
    {
        public const string CreateBrandFailMessage = "Create new brand failed";
        public const string CreateBrandSucceedMessage = "Create new brand successful";
        public const string BrandNotFoundMessage = "Brand does not exist in system";
        public const string EmptyBrandIdMessage = "Brand Id bị trống";
    }
    public static class Account
    {
        public const string CreateAccountWithWrongRoleMessage = "Please create with acceptent role";
        public const string CreateBrandAccountFailMessage = "Create brand account failed";
        public const string UpdateAccountStatusRequestWrongFormatMessage = "Cập nhật status tài khoản request sai format";
        public const string AccountNotFoundMessage = "Không tìm thấy tài khoản";
        public const string UpdateAccountStatusSuccessfulMessage = "Cập nhật status tài khoản thành công";
        public const string UpdateAccountStatusFailedMessage = "Cập nhật status tài khoản thất bại";
    }

    public static class Store
    {
        public const string EmptyStoreIdMessage = "Store Id bị trống";
        public const string CreateStoreFailMessage = "Create new store failed";
    }
}