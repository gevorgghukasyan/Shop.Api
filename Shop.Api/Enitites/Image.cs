using System.ComponentModel.DataAnnotations;

namespace Shop.Api.Enitites;

public class Image
{
	[Key]
	public int Id { get; set; }
	[Required]
	public string FileName { get; set; }
	[Required]
	public string ContentType { get; set; }
	[Required]
	public byte[] Data { get; set; }
	public DateTime UploadAt { get; set; }
}
