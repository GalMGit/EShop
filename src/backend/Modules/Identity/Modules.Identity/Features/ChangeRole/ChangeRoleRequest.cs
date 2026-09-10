namespace Modules.Identity.Features.ChangeRole;

public  sealed record ChangeRoleRequest(
    Guid UserId, 
    Guid RoleId);