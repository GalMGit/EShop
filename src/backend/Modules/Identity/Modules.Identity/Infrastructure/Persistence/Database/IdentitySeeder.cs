using Microsoft.EntityFrameworkCore;
using Modules.Identity.Domain;
using Modules.Identity.Infrastructure.Persistence.Database.Context;

namespace Modules.Identity.Infrastructure.Persistence.Database;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        IdentityDbContext db,
        CancellationToken ct = default)
    {
        var permissionNames = new[]
        {
            "products.read",
            "products.write",

            "orders.read",
            "orders.manage",

            "stock.read",
            "stock.manage",
            
            "users.read",
            "users.manage"
        };

        var permissions = await db.Permissions
            .ToDictionaryAsync(x => x.Name, ct);

        foreach (var permissionName in permissionNames)
        {
            if (permissions.ContainsKey(permissionName))
                continue;

            var permission = new Permission
            {
                Id = Guid.NewGuid(),
                Name = permissionName
            };

            db.Permissions.Add(permission);
            permissions.Add(permissionName, permission);
        }

        var roleNames = new[]
        {
            "Customer",
            "Manager",
            "Warehouse",
            "Admin"
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
                Id = Guid.NewGuid(),
                Name = roleName
            };

            db.Roles.Add(role);
            roles.Add(roleName, role);
        }

        await db.SaveChangesAsync(ct);
        
        await AddPermissionsAsync(
            roles["Manager"],
            permissions,
            [
                "products.read",
                "products.write",
                "orders.read",
                "orders.manage"
            ]);

        await AddPermissionsAsync(
            roles["Warehouse"],
            permissions,
            [
                "products.read",
                "stock.read",
                "stock.manage"
            ]);

        await AddPermissionsAsync(
            roles["Customer"],
            permissions,
            [
                "products.read",
                "orders.read"
            ]);
        
        await AddPermissionsAsync(
            roles["Admin"],
            permissions,
            permissions.Keys);

        await db.SaveChangesAsync(ct);
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
}