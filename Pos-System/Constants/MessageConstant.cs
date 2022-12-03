namespace Pos_System.API.Constants;

public static class MessageConstant
{
    public static class LoginMessage
    {
        public const string InvalidUsernameOrPassword = "Username or password is not correct";
        public const string DeactivatedAccount = "Deactivated account can not login system";
    }
    public static class CreateNewBrandMessage
    {
        public const string FailMessage = "Create new brand failed";
        public const string SucceedMessage = "Create new brand successful";
    }
}