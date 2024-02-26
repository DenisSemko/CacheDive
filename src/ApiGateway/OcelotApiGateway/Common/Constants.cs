namespace OcelotApiGateway.Common;

public class Constants
{
    public class AuthRequestPaths
    {
        public const string LoginPath = "Auth/login";
        public const string RegistrationPath = "Auth/registration";
        public const string RefreshPath = "https://localhost:8006/api/Auth/{0}";
    }
}