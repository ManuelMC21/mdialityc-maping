using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite;
using NetTopologySuite.Geometries;

public static class MunicipalityEndpoint
{
    public static void MapMunicipalityEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/municipalities", [Authorize(Roles = "Admin")] async (MunicipalityDto dto, AppDbContext db, HttpContext httpContext, UserManager<ApplicationUser> userManager) =>
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

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory();

            try
            {
                if (dto.Coordinates == null || dto.Coordinates.Count < 3)
                {
                    return Results.BadRequest("A polygon must have at least 3 coordinates");
                }

                var coordinates = dto.Coordinates
                    .Select(c => new Coordinate(c[0], c[1]))
                    .ToArray();

                if (!coordinates.First().Equals(coordinates.Last()))
                {
                    coordinates.Append(coordinates.First()).ToArray();
                }

                var linearRing = geometryFactory.CreateLinearRing(coordinates);
                var polygon = geometryFactory.CreatePolygon(linearRing);

                var municipality = new Municipality
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    ProvinceId = dto.ProvinceId,
                    Geom = polygon
                };

                db.Municipalities.Add(municipality);
                await db.SaveChangesAsync();

                return Results.Ok("Municipality has been Created");
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.InnerException?.Message);
            }
        })
        .WithTags("Municipalities");
    }
}