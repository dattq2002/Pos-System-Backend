namespace Pos_System.API.Constants;

public static class ApiEndPointConstant
{
    static ApiEndPointConstant()
    {

    }

    public const string RootEndPoint = "/api";
    public const string ApiVersion = "/v1";
    public const string ApiEndpoint = RootEndPoint + ApiVersion;

    public static class Authentication
    {
        public const string AuthenticationEndpoint = ApiEndpoint + "/auth";
        public const string Login = AuthenticationEndpoint + "/login";
    }

    public static class Brand
    {
	    public const string BrandEndpoint = ApiEndpoint + "/brands";
	    public const string CreateBrandAccountEndpoint = BrandEndpoint + "/{id}/users";
    }
}