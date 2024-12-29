using Microsoft.AspNetCore.Mvc;

public static class ImageEndpoint
{
    public static void MapImageEndpoints(this IEndpointRouteBuilder app)
    {

        app.MapPost("/entity/{id}/upload", async (int id, HttpRequest request, AppDbContext db) =>
        {

            if (!request.HasFormContentType)
            {
                return Results.BadRequest("The Content-Type must be multipart/form-data");
            }

            var entity = await db.Entities.FindAsync(id);
            if (entity == null)
            {
                return Results.NotFound("Entity Not Found");
            }

            var formCollection = await request.ReadFormAsync();
            var file = formCollection.Files.FirstOrDefault();

            if (file == null || file.Length == 0)
            {
                return Results.BadRequest("You must select one file");
            }

            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

            var entityFolderPath = Path.Combine(basePath, id.ToString());
            if (!Directory.Exists(entityFolderPath))
            {
                Directory.CreateDirectory(entityFolderPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(entityFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            entity.ImagesUrl = $"/images/{entityFolderPath}";
            await db.SaveChangesAsync();
            var fileUrl = $"/uploads/{id}/{fileName}";
            return Results.Ok(new
            {
                Message = "File Uploaded",
                Url = fileUrl
            });

        }).WithName("Upload")
          .WithMetadata(new ConsumesAttribute("multipart/form-data"))
          .WithTags("Images");

        app.MapGet("/images/{id}", (int id) =>
        {
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var folderPath = Path.Combine(uploadsPath, id.ToString());

            if (!System.IO.Directory.Exists(folderPath))
            {
                return Results.NotFound("Folder not found");
            }

            return Results.Ok(folderPath);
        }).WithTags("Images");

    }
}