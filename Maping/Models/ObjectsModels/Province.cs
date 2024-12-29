using NetTopologySuite.Geometries;

public class Province
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Geometry Geom { get; set; }
    public ICollection<Municipality>? Municipalities { get; set; }
}