using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Data;
using Shop.Api.Enitites;

namespace Shop.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
	private readonly ProductDataContext _context; // Replace with your DbContext

	public ProductsController(ProductDataContext context)
	{
		_context = context;
	}

	// GET: api/Products
	[HttpGet]
	public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
	{
		var sss =  await _context.Products
			.Include(p => p.Image) // Include related Image
			.ToListAsync();

		return sss;
	}

	// GET: api/Products/5
	[HttpGet("{id}")]
	public async Task<ActionResult<Product>> GetProduct(int id)
	{
		var product = await _context.Products
			.Include(p => p.Image)
			.FirstOrDefaultAsync(p => p.Id == id);

		if (product == null)
		{
			return NotFound();
		}

		return product;
	}

	// POST: api/Products
	[HttpPost]
	public async Task<ActionResult<Product>> CreateProduct([FromBody] ProductDto product)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		_context.Products.Add(new Product() { Name = product.Name, Price = product.Price, Title = product.Title, Description = product.Description });
		await _context.SaveChangesAsync();

		return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
	}

	// PUT: api/Products/5
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
	{
		if (id != product.Id)
		{
			return BadRequest();
		}

		_context.Entry(product).State = EntityState.Modified;

		try
		{
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!ProductExists(id))
			{
				return NotFound();
			}
			throw;
		}

		return NoContent();
	}

	// DELETE: api/Products/5
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteProduct(int id)
	{
		var product = await _context.Products.FindAsync(id);
		if (product == null)
		{
			return NotFound();
		}

		_context.Products.Remove(product);
		await _context.SaveChangesAsync();

		return NoContent();
	}

	private bool ProductExists(int id)
	{
		return _context.Products.Any(e => e.Id == id);
	}

	[HttpPost("{id}/image")]
	public async Task<IActionResult> UploadImage(int id, IFormFile file)
	{
		var product = await _context.Products.FindAsync(id);
		if (product == null)
		{
			return NotFound();
		}

		if (file == null || file.Length == 0)
		{
			return BadRequest("No file uploaded.");
		}

		using var memoryStream = new MemoryStream();
		await file.CopyToAsync(memoryStream);

		var image = new Image
		{
			FileName = file.FileName,
			ContentType = file.ContentType,
			Data = memoryStream.ToArray(),
			UploadAt = DateTime.UtcNow
		};

		product.Image = image;
		await _context.SaveChangesAsync();

		return Ok(image);
	}
}
