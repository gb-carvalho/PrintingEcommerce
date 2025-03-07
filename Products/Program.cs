using Microsoft.EntityFrameworkCore;
using Products.Data;

var builder = WebApplication.CreateBuilder(args);

var corsPolicy = "AllowSpecificOrigins";
builder.Services.AddCors(options =>
{
	options.AddPolicy(corsPolicy,
		policy => policy.WithOrigins("http://localhost:51745")
			.AllowAnyHeader()
			.AllowAnyMethod());
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicy);
app.UseAuthorization();

app.MapControllers();

app.Run();
