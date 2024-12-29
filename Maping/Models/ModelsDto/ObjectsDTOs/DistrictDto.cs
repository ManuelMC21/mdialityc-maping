public class DistrictDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int MunicipalityId { get; set; }
    public List<List<double>> Coordinates { get; set; }
}