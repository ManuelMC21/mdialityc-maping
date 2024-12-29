public class Restaurant
{
    public int RestaurantId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int EntityId { get; set; }

    public Entity Entity { get; set; }
}