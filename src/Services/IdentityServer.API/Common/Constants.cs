namespace IdentityServer.API.Common;

public static class Constants
{
    public class Errors
    {
        public const string UsernameAlreadyExists = "User with such username already exists";
        public const string UserDoesNotExist = "User does not exist";
        public const string WrongCredentials = "User credentials are wrong";
    }

    public class Roles
    {
        public const string User = "User";
    }
    
    public class Logs
    {
        public const string UserDeleted = "User {0} is successfully deleted";
        public const string UserUpdated = "User {0} is successfully updated";
    }
}