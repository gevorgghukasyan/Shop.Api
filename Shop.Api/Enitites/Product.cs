// Ignore Spelling: Dto

using System.Text.Json.Serialization;

namespace Shop.Api.Enitites;

public class Product
{
	public int Id { get; set; }
	public string Name { get; set; }
	public float Price { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	//[JsonIgnore]
	public int? ImageId { get; set; }
	public Image Image { get; set; }
}

public class ProductDto
{
	public int Id { get; set; }
	public string Name { get; set; }
	public float Price { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
}
