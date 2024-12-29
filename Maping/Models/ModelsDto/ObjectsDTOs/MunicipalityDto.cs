public class MunicipalityDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProvinceId { get; set; }
    public List<List<double>> Coordinates { get; set; }
}