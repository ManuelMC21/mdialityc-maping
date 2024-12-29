using Microsoft.EntityFrameworkCore;

public static class FormEndpoint
{
    public static void MapFormEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/new-form", async (FormDto dto, AppDbContext db) =>
        {
            if (string.IsNullOrEmpty(dto.Name) || dto.Fields == null || !dto.Fields.Any())
            {
                return Results.BadRequest("The form must be a name and at least a field");
            }

            var form = new Form
            {
                Name = dto.Name,
                Fields = dto.Fields.Select(f => new FormField
                {
                    Name = f.Name,
                    Type = f.Type,
                    IsRequired = f.IsRequired
                }).ToList()
            };

            db.Forms.Add(form);
            await db.SaveChangesAsync();

            return Results.Ok(new
            {
                Message = "Form Has Been Created",
                Id = form.Id
            });

        }).WithTags("Form");

        app.MapGet("/api/forms", async (AppDbContext db) =>
        {
            var forms = await db.Forms
                .Include(f => f.Fields)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    Fields = f.Fields.Select(field => new
                    {
                        field.Id,
                        field.Name,
                        field.Type,
                        field.IsRequired
                    })
                })
                .ToListAsync();

            return Results.Ok(forms);
        }).WithTags("Form");
    }
}