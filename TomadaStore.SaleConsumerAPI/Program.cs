using TomadaStore.SaleConsumerAPI.Data;
using TomadaStore.SaleConsumerAPI.Repository;
using TomadaStore.SaleConsumerAPI.Repository.Interface;
using TomadaStore.SaleConsumerAPI.Services;
using TomadaStore.SaleConsumerAPI.Services.Interface;
using TomadaStore.Utils.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<ConnectionDB>();
builder.Services.AddSingleton<SaleConverter>();

builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleConsumerService, SaleConsumerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
