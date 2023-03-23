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
        public const string BrandNotFoundMessage = "Brand không tồn tại trong hệ thống";
        public const string EmptyBrandIdMessage = "Brand Id bị trống";
        public const string UpdateBrandSuccessfulMessage = "Cập nhật thông tin brand thành công";
        public const string UpdateBrandFailedMessage = "Cập nhật thông tin brand thất bại";
    }

    public static class Account
    {
        public const string CreateAccountWithWrongRoleMessage = "Please create with acceptent role";
        public const string CreateBrandAccountFailMessage = "Create brand account failed";
        public const string CreateStaffAccountFailMessage = "Create staff account failed";
        public const string UserUnauthorizedMessage = "Bạn không được phép cập nhật status cho tài khoản này";

        public const string UpdateAccountStatusRequestWrongFormatMessage =
            "Cập nhật status tài khoản request sai format";

        public const string AccountNotFoundMessage = "Không tìm thấy tài khoản";
        public const string UpdateAccountStatusSuccessfulMessage = "Cập nhật status tài khoản thành công";
        public const string UpdateAccountStatusFailedMessage = "Cập nhật status tài khoản thất bại";
        public const string EmptyAccountId = "Account id bị trống";
    }

    public static class Store
    {
        public const string EmptyStoreIdMessage = "Store Id bị trống";
        public const string CreateStoreFailMessage = "Create new store failed";
        public const string StoreNotFoundMessage = "Store không tồn tại trong hệ thống";
        public const string UpdateStoreInformationSuccessfulMessage = "Cập nhật thông tin store thành công";
        public const string UpdateStaffInformationSuccessfulMessage = "Cập nhật thông tin staff thành công";
        public const string CreateNewStoreAccountUnauthorizedMessage = "Bạn không có quyền tạo tài khoản cho một store";
        public const string StoreNotInBrandMessage = "Bạn không được phép tạo account nằm ngoài brand đang quản lí";
        public const string GetStoreOrdersUnAuthorized = "Bạn không được phép lấy orders của store khác!";
    }

    public static class Category
    {
        public const string CreateNewCategoryFailedMessage = "Tạo mới Category bị failed";
        public const string EmptyCategoryIdMessage = "Category Id bị trống";
        public const string CategoryNotFoundMessage = "Category không có trong hệ thống";
        public const string UpdateCategorySuccessfulMessage = "Category được cập nhật thành công";
        public const string UpdateCategoryFailedMessage = "Category cập nhật thất bại";
        public const string UpdateExtraCategorySuccessfulMessage = "Extra Category được cập nhật thành công";
        public const string UpdateExtraCategoryFailedMessage = "Extra Category cập nhật thất bại";
    }

    public static class Collection
    {
        public const string EmptyCollectionIdMessage = "Collection Id bị trống";
        public const string CollectionNotFoundMessage = "Collection không tồn tại trong hệ thống";
        public const string CreateNewCollectionFailedMessage = "Tạo mới collection thất bại";
        public const string UpdateProductInCollectionSuccessfulMessage = "Product collection trong được cập nhật thành công";
        public const string UpdateProductInCollectionFailedMessage = "Product collection cập nhật thất bại";
    }

    public static class Product
    {
        public const string CreateNewProductFailedMessage = "Tạo mới product thất bại";
        public const string UpdateProductFailedMessage = "Cập nhật thông tin product thất bại";
        public const string EmptyProductIdMessage = "Product Id bị trống";
        public const string ProductNotFoundMessage = "Product không tồn tại trong hệ thống";
    }

    public static class Menu
    {
        public const string CreateNewMenuFailedMessage = "Tạo mới Menu thất bại";
        public const string EmptyMenuIdMessage = "Id của menu không hợp lệ.";
        public const string MenuNotFoundMessage = "Menu không tồn tại trong hệ thống";
        public const string ProductNotInBrandMessage = "Bạn không thể thêm product của brand khác, product id: ";
        public const string MissingStoreIdMessage = "Bạn đang không thuộc về một store, không thể lấy menu";
        public const string BrandIdWithMenuIdIsNotExistedMessage = "BrandId và MenuId không tồn tại trong hệ thống";
        public const string BaseMenuIsExistedInBrandMessage = "Brand đã tồn tại menu cơ bản, không thể cập nhật priority là 0";
        public const string UpdateMenuInformationSuccessfulMessage = "Cập nhật thông tin menu thành công";
        public const string UpdateMenuInformationFailedMessage = "Cập nhật thông tin menu thất bại";
        public const string EndTimeLowerThanStartTimeMessage =
            "Thời gian kết thúc không được nhỏ hơn thời gian bắt đầu";

        public const string BaseMenuExistedMessage = "Nhãn hàng đã có menu cơ bản";
    }

    public static class Order
    {
        public const string UserNotInSessionMessage = "Tài khoản không trong ca làm để tạo Order";
        public const string NoProductsInOrderMessage = "Không thể tạo order khi order không đính kèm sản phẩm bên trong";
        public const string CreateOrderFailedMessage = "Tạo mới order thất bại";
        public const string EmptyOrderIdMessage = "Id của order không hợp lệ";
        public const string OrderNotFoundMessage = "Order không tồn tại trong hệ thống";
    }
}