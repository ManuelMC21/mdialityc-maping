using NetTopologySuite.Geometries;

public class Municipality
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProvinceId { get; set; }
    public Geometry Geom { get; set; }
    public Province? Province { get; set; }
    public ICollection<District> Districts { get; set; }
}