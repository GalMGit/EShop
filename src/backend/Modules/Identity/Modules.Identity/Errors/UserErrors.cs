using EShop.Shared.ResultType;

namespace Modules.Identity.Errors;

public static class UserErrors
{
    public static readonly Error NotFound =
        Error.NotFound(
            "user.not_found", 
            "User was not found.");
}