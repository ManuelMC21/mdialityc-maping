using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class EntityTypeEndpoint
{
    public static void MapEntityTypeEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/entity-types", [Authorize(Roles = "Admin")] async (ENtityTypeDto ent, HttpContext httpContext, AppDbContext db, UserManager<ApplicationUser> userManager) =>
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null || !await userManager.IsInRoleAsync(user, "Admin"))
            {
                return Results.Forbid();
            }

            var type = new EntityType
            {
                EntityTypeId = ent.EntityTypeId,
                Name = ent.Name
            };

            if (type.Name == "")
            {
                return Results.BadRequest("The EntityType must have a Name");
            }

            db.Types.Add(type);
            await db.SaveChangesAsync();

            return Results.Ok("EntityType created");
        })
        .WithTags("EntityType");

        app.MapDelete("/api/delete-entity-type", async (int id, AppDbContext db) =>
        {
            var type = await db.Types.FindAsync(id);
            if (type == null)
            {
                return Results.NotFound($"EntityType with id = {id} not found");
            }

            db.Types.Remove(type);
            await db.SaveChangesAsync();

            return Results.Ok("EntityType deleted");
        })
        .WithTags("EntityType");

        app.MapGet("/api/entity-types", async (AppDbContext db) =>
        {
            var types = await db.Types.ToListAsync();

            return Results.Ok(types);
        })
        .WithTags("EntityType");

        app.MapGet("/api/entity-types-by-id", async (int id, AppDbContext db) =>
        {
            var type = await db.Types.FindAsync(id);
            if (type == null)
            {
                return Results.NotFound($"EntityType with id = {id} not found");
            }

            return Results.Ok(type.Name);
        })
        .WithTags("EntityType");
    }
}