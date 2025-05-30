using Microsoft.EntityFrameworkCore;
using Shop.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductDataContext>(options =>
	options.UseSqlServer("Data Source=.;Initial Catalog=ShopDb;Integrated Security=True;Encrypt=False"));
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
using (var context = new ProductDataContext())
{
	context.Database.EnsureCreated();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
