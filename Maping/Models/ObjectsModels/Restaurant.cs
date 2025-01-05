using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Restaurant
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonIgnore]
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int EntityId { get; set; }

    public Entity Entity { get; set; }
}