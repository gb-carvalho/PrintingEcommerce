using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Users.Domain.Entities;
using Users.Infraestructure.Data;
using Users.Application.Services;
using Users.Domain.Interfaces;
using Users.Infraestructure.Repositories;


var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

var corsPolicy = "AllowSpecificOrigins";
builder.Services.AddCors(options =>
{
	options.AddPolicy(corsPolicy,
		policy => policy.WithOrigins("http://localhost:51745")
			.AllowAnyHeader()
			.AllowAnyMethod());
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
	builder.Configuration.GetConnectionString("DefaultConnection")
	));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

// Adicionar Identity
builder.Services.AddIdentity<User, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders()
	.AddRoles<IdentityRole>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1", new OpenApiInfo { Title = "Minha API", Version = "v1" });

	// Configuração do Swagger para aceitar token JWT
	var securityScheme = new OpenApiSecurityScheme
	{
		Name = "Authorization",
		Description = "Insira o token JWT no formato: Bearer {seu token}",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT"
	};

	// Requerer o token JWT em todas as requisições
	var securityRequirement = new OpenApiSecurityRequirement	
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = JwtBearerDefaults.AuthenticationScheme
				}
			}, new string []{ }
		}
	};

	options.AddSecurityDefinition("Bearer", securityScheme);
	options.AddSecurityRequirement(securityRequirement);
});


builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtSettings["Issuer"],
			ValidAudience = jwtSettings["Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"] ?? ""))
		};
	});
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
	options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

var app = builder.Build();	

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(corsPolicy);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using(var scope = app.Services.CreateScope())
{
	RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	UserService userService = scope.ServiceProvider.GetRequiredService<UserService>();

	await userService.EnsureRolesCreated();
	await userService.EnsureAdminCreated();
}

app.Run();