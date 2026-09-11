using EShop.Shared.Names;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Modules.Identity.Domain;
using Modules.Identity.Infrastructure.Persistence.Database.Context;

namespace Modules.Identity.Infrastructure.Persistence.Database;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        IdentityDbContext db,
        IOptions<AdminOptions> options,
        CancellationToken ct = default)
    {
        AdminOptions adminOptions = options.Value;
        var permissionNames = new[]
        {
            PermissionNames.ProductsRead,
            PermissionNames.ProductsWrite,
            
            PermissionNames.BrandRead,
            PermissionNames.BrandWrite,
            
            PermissionNames.CategoryRead,
            PermissionNames.CategoryWrite,

            PermissionNames.OrdersRead,
            PermissionNames.OrdersManage,

            PermissionNames.StockRead,
            PermissionNames.StockManage,
            
            PermissionNames.UsersRead,
            PermissionNames.UsersManage
        };

        var permissions = await db.Permissions
            .ToDictionaryAsync(x => x.Name, ct);

        foreach (var permissionName in permissionNames)
        {
            if (permissions.ContainsKey(permissionName))
                continue;

            var permission = new Permission
            {
                Id = Guid.CreateVersion7(),
                Name = permissionName
            };

            db.Permissions.Add(permission);
            permissions.Add(permissionName, permission);
        }

        var roleNames = new[]
        {
            RoleNames.Customer,
            RoleNames.CatalogManager,
            RoleNames.OrderManager,
            RoleNames.Warehouse,
            RoleNames.Admin,
        };

        var roles = await db.Roles
            .Include(x => x.Permissions)
            .ToDictionaryAsync(x => x.Name, ct);

        foreach (var roleName in roleNames)
        {
            if (roles.ContainsKey(roleName))
                continue;

            var role = new Role
            {
                Id = Guid.CreateVersion7(),
                Name = roleName
            };

            db.Roles.Add(role);
            roles.Add(roleName, role);
        }

        await db.SaveChangesAsync(ct);
        
        await AddPermissionsAsync(
            roles[RoleNames.CatalogManager],
            permissions,
            [
                PermissionNames.ProductsRead,
                PermissionNames.ProductsWrite,
                PermissionNames.BrandRead,
                PermissionNames.BrandWrite,
                PermissionNames.CategoryRead,
                PermissionNames.CategoryWrite
            ]);
        
        await AddPermissionsAsync(
            roles[RoleNames.OrderManager],
            permissions,
            [
                PermissionNames.OrdersRead,
                PermissionNames.OrdersManage,
            ]);

        await AddPermissionsAsync(
            roles[RoleNames.Warehouse],
            permissions,
            [
                PermissionNames.ProductsRead,
                PermissionNames.StockRead,
                PermissionNames.StockManage,
            ]);

        await AddPermissionsAsync(
            roles[RoleNames.Customer],
            permissions,
            [
                PermissionNames.ProductsRead,
                PermissionNames.OrdersRead,
            ]);
        
        await AddPermissionsAsync(
            roles[RoleNames.Admin],
            permissions,
            permissions.Keys);

        await db.SaveChangesAsync(ct);
        
        await SeedAdminAsync(db,
            roles[RoleNames.Admin],
            adminOptions,
            ct);
    }

    private static Task AddPermissionsAsync(
        Role role,
        IReadOnlyDictionary<string, Permission> permissions,
        IEnumerable<string> permissionNames)
    {
        var existingPermissionIds = role.Permissions
            .Select(x => x.Id)
            .ToHashSet();

        foreach (var permissionName in permissionNames)
        {
            var permission = permissions[permissionName];

            if (existingPermissionIds.Contains(permission.Id))
                continue;

            role.Permissions.Add(permission);
        }

        return Task.CompletedTask;
    }
    
    private static async Task SeedAdminAsync(
        IdentityDbContext db,
        Role adminRole,
        AdminOptions options,
        CancellationToken ct)
    {
        var adminExists = await db.Users
            .AnyAsync(x => x.Email == options.Email, ct);

        if (adminExists)
            return;

        var admin = new User
        {
            Id = Guid.CreateVersion7(),
            Username = options.Username,
            Email = options.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(options.Password),
            CreatedAt = DateTime.UtcNow,
            Roles = [adminRole]
        };

        db.Users.Add(admin);

        await db.SaveChangesAsync(ct);
    }
}