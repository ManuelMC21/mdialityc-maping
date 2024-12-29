using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

public static class EntityEndpoint
{
    public static double DegToRad(double degrees) => degrees * (Math.PI / 180);

    public static void MapEntityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/entities", [Authorize(Roles = "Admin,User")] async (EntityDto dto, UserManager<ApplicationUser> userManager, HttpContext httpContext, AppDbContext db) =>
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory();
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Results.Unauthorized();

            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return Results.NotFound("User not found");

            try
            {
                if (dto.latitude > 180 || dto.latitude < -180 || dto.longitude > 180 || dto.longitude < -180)
                {
                    return Results.BadRequest("Check latitude and longitude");
                }

                var point = geometryFactory.CreatePoint(new Coordinate(dto.longitude, dto.latitude));

                var entity = new Entity
                {
                    DistrictId = dto.DistrictId,
                    EntityTypeId = dto.EntityTypeId,
                    UserId = user.Id,
                    Geom = point,
                };

                db.Entities.Add(entity);
                await db.SaveChangesAsync();

                return Results.Ok(new
                {
                    message = "Entity has been created",
                    retId = entity.Id
                });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.InnerException?.Message);
            }
        })
        .WithTags("Entity");

        app.MapGet("/api/entities/{id}", async (int id, AppDbContext db) =>
        {
            var entities = await db.Entities
                .Where(e => e.Id == id)
                .Select(e => new
                {
                    Id = e.Id,
                    DistrictId = e.DistrictId,
                    latitude = e.Geom.Coordinate.X,
                    longitude = e.Geom.Coordinate.Y
                })
                .ToListAsync();

            return Results.Ok(entities);

        }).WithTags("Entity");

        app.MapGet("/api/entities", async (AppDbContext db) =>
        {
            var entities = await db.Entities
                .Select(e => new
                {
                    Id = e.Id,
                    DistrictId = e.DistrictId,
                    latitude = e.Geom.Coordinate.X,
                    longitude = e.Geom.Coordinate.Y,
                })
                .ToListAsync();

            return Results.Ok(entities);
        })
        .WithTags("Entity");

        app.MapGet("/api/user/entities", [Authorize(Roles = "Admin, User")] async (UserManager<ApplicationUser> userManager, HttpContext httpContext, AppDbContext db) =>
        {
            var userId = httpContext.User.FindFirst("sub")?.Value;
            if (userId == null) return Results.Unauthorized();

            var entities = await db.Entities
                .Where(e => e.UserId == userId)
                .ToListAsync();

            return Results.Ok(entities);
        })
        .WithTags("Entity");
    }
}

