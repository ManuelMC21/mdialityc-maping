using NetTopologySuite.Geometries;

public class District
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MunicipalityId { get; set; }
    public Geometry Geom { get; set; }
    public Municipality? Municipality { get; set; }
    public ICollection<Entity> Entities { get; set; }
}