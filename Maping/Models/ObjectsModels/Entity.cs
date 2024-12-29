using NetTopologySuite.Geometries;

public class Entity
{
    public int Id { get; set; }
    public Point Geom { get; set; }

    public string UserId { get; set; }
    public int DistrictId { get; set; }
    public int EntityTypeId { get; set; }
    public string? ImagesUrl { get; set; }

    //Navigation Variables
    public ApplicationUser user { get; set; }
    public EntityType EntityType { get; set; }
    public District District { get; set; }
    public Restaurant Restaurant { get; set; }
}