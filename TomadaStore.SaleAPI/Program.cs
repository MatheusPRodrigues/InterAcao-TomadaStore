using TomadaStore.SaleAPI.Data;
using TomadaStore.SaleAPI.Repository;
using TomadaStore.SaleAPI.Repository.Interfaces;
using TomadaStore.SaleAPI.Services.Interfaces.v1;
using TomadaStore.SaleAPI.Services.v1;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<ConnectionDB>();

builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

builder.Services.AddHttpClient("ProductAPI", client =>
    client.BaseAddress = new Uri("https://localhost:6001/api/v1/Product/"));

builder.Services.AddHttpClient("CustomerAPI", client =>
    client.BaseAddress = new Uri("https://localhost:5001/api/v1/Customer/"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
