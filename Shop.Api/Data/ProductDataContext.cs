using Shop.Api.Enitites;
using Microsoft.EntityFrameworkCore;

namespace Shop.Api.Data;

public class ProductDataContext : DbContext
{
	public DbSet<Product> Products { get; set; }
	public DbSet<Image> Images { get; set; }

	public ProductDataContext(DbContextOptions<ProductDataContext> options)
		: base(options)
	{
	}

    public ProductDataContext()
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Product>(entity =>
		{
			entity.HasKey(p => p.Id);
			entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
			entity.Property(p => p.Title).HasMaxLength(200);
			entity.Property(p => p.Description).HasMaxLength(1000);

			// Configure one-to-one relationship with Image
			entity.HasOne(p => p.Image)
				  .WithOne()
				  .HasForeignKey<Product>(p => p.ImageId)
				  .OnDelete(DeleteBehavior.Cascade);
		});

		// Configure Image entity
		modelBuilder.Entity<Image>(entity =>
		{
			entity.HasKey(i => i.Id);
			entity.Property(i => i.FileName).IsRequired().HasMaxLength(255);
			entity.Property(i => i.ContentType).IsRequired().HasMaxLength(100);
			entity.Property(i => i.Data).IsRequired();
		});
	}
}
