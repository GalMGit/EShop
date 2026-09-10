using EShop.Shared.ResultType;

namespace Modules.Identity.Errors;

public static class RoleErrors
{
    public static readonly Error NotFound
        = Error.NotFound(
            "identity.role_not_found",
            "Роль с таким Id не найдена.");
}