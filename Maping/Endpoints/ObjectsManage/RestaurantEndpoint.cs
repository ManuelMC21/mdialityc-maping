using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class RestaurantEndpoint
{
    public static void MapRestaurantEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/create-restaurant", async (RestaurantDto dto, AppDbContext db) =>
        {
            try
            {
                var restaurant = new Restaurant
                {
                    RestaurantId = dto.RestaurantId,
                    EntityId = dto.EntityId,
                    Name = dto.Name,
                    Description = dto.Description
                };

                db.Restaurants.Add(restaurant);
                await db.SaveChangesAsync();

                return Results.Ok("Restaurant created");
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.InnerException?.Message);
            }
        })
        .WithTags("Restaurant");

        app.MapDelete("/api/delete-restaurant", [Authorize(Roles = "Admin,User")] async (int id, AppDbContext db, HttpContext httpContext, UserManager<ApplicationUser> userManager) =>
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Results.Forbid();
            }

            var restaurant = await db.Restaurants
                .Include(r => r.Entity)
                .FirstOrDefaultAsync(r => r.RestaurantId == id);

            if (restaurant == null)
            {
                return Results.NotFound("Restaurant Not Found");
            }

            if (restaurant.Entity == null)
            {
                return Results.BadRequest("The Entity Associated with the restaurant was no found");
            }

            if (userId != restaurant.Entity?.UserId && !await userManager.IsInRoleAsync(user, "Admin"))
            {
                return Results.BadRequest("You not the owner of the entity");
            }

            db.Entities.Remove(restaurant.Entity);
            db.Restaurants.Remove(restaurant);
            await db.SaveChangesAsync();

            return Results.Ok("Restaurant deleted");
        })
        .WithTags("Restaurant");
    }
}