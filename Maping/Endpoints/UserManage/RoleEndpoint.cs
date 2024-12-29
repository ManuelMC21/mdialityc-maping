using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

public static class RoleEndpoint
{
    public static void MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/assign-role", async (string userId, string roleName, UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Results.NotFound("User not found");
            }

            if (!await userManager.IsInRoleAsync(user, roleName))
            {
                var result = await userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors);
                }
            }

            return Results.Ok($"Role {roleName} assigned");
        })
        .WithTags("Role");
    }
}