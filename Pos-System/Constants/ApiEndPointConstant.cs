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
	    public const string BrandsEndpoint = ApiEndpoint + "/brands";
	    public const string BrandEndpoint = BrandsEndpoint + "/{id}";
	    public const string BrandAccountEndpoint = BrandEndpoint + "/users";
	    public const string StoresInBrandEndpoint = BrandEndpoint + "/stores";
    }

    public static class Store
    {
	    public const string StoresEndpoint = ApiEndpoint + "/stores";
	    public const string StoreEndpoint = StoresEndpoint + "/{id}";
    }
}