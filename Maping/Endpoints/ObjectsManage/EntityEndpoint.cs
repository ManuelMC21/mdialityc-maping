using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.Utilities;

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

        app.MapGet("/api/entity/{id}", async (int id, AppDbContext db) =>
        {
            var entity = await db.Entities.FindAsync(id);

            if (entity == null)
            {
                return Results.NotFound("Entity Not Found");
            }

            var retEntity = new
            {
                Id = entity.Id,
                DistrictId = entity.DistrictId,
                Latitude = entity.Geom.Coordinate.X,
                Longitude = entity.Geom.Coordinate.Y
            };

            return Results.Ok(retEntity);

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

        app.MapGet("/api/entities/nearby", async (double lat, double lng, double distance, AppDbContext db) =>
        {
            const double EarthRadiusKm = 6371;

            var entities = await db.Entities
                .Select(e => new
                {
                    Id = e.Id,
                    DistrictId = e.DistrictId,
                    latitude = e.Geom.Coordinate.X,
                    longitude = e.Geom.Coordinate.Y
                }).ToListAsync();

            var nearbyEntities = entities
                .Where(e =>
                {
                    var dLat = DegressToRadians(e.latitude - lat);
                    var dLon = DegressToRadians(e.longitude - lng);
                    var lat1 = DegressToRadians(lat);
                    var lat2 = DegressToRadians(e.latitude);

                    var a = Math.Pow(Math.Sin(dLat / 2), 2) + Math.Pow(Math.Sin(dLon / 2), 2)
                        * Math.Cos(lat1) * Math.Cos(lat2);

                    var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    var distanceKm = EarthRadiusKm * c;

                    return distanceKm <= distance / 1000;
                })
                .ToList();

            return Results.Ok(nearbyEntities);
        }).WithTags("Entity");
    }

    private static double DegressToRadians(double degress)
    {
        return degress * Math.PI / 180;
    }
}

