namespace Contract;

public class PropertyDto
{
	public int PropertyId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Address { get; set; }
	public decimal Price { get; set; }
	public string PropertyType { get; set; }
	public string Status { get; set; }
	public double Area { get; set; }
	public ICollection<PropertyImageDto> Images { get; set; } = new List<PropertyImageDto>();
}

public class PropertyCreateDto
{
    public int PropertyId { get; set; }
    public string Title { get; set; }
	public string Description { get; set; }
	public string Address { get; set; }
	public decimal Price { get; set; }
	public string PropertyType { get; set; }
	public string Status { get; set; }
	public double Area { get; set; }
}
public class PropertyUpdateDto
{
	public int PropertyId { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Address { get; set; }
	public decimal Price { get; set; }
	public string PropertyType { get; set; }
	public string Status { get; set; }
	public double Area { get; set; }
	public List<int> ImageIds { get; set; } = new List<int>();
}
