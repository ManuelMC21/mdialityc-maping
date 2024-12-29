using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; }

    [JsonIgnore]
    public List<Entity> entities { get; set; }
}