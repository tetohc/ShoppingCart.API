using Microsoft.EntityFrameworkCore;
using ShoppingCart.API.Business;
using ShoppingCart.API.Business.Interfaces;
using ShoppingCart.API.Database;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnectionString")));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Version = "v1",
        Title = "ShoppingCart API",
        Description = "ASP.NET Core Web API para gestión de un carrito de compras",
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<ICategoryBusiness, CategoryBusiness>();
builder.Services.AddScoped<IProductBusiness, ProductBusiness>();
builder.Services.AddScoped<IFileStorageBusiness, LocalFileStorageBusiness>();
builder.Services.AddScoped<IOrderBusiness, OrderBusiness>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ShoppingCartPolicy", policyy =>
    {
        policyy.WithOrigins("http://localhost:4200")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseStaticFiles();
app.UseCors("ShoppingCartPolicy");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingCart API V1");
    c.RoutePrefix = string.Empty;
});

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
