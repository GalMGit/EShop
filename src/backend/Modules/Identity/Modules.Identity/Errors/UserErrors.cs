using EShop.Shared.ResultType;

namespace Modules.Identity.Errors;

public static class UserErrors
{
    public static readonly Error NotFound =
        Error.NotFound(
            "user.not_found", 
            "User was not found.");
    
    public static readonly Error RegistrationPending =
        Error.Conflict(
            "user.registration_pending",
            "На этот email уже отправлен код подтверждения. Повторите попытку через 5 минут.");
}