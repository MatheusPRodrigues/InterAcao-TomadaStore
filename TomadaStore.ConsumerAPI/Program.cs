using TomadaStore.ConsumerAPI.Data;
using TomadaStore.ConsumerAPI.Repository;
using TomadaStore.ConsumerAPI.Repository.Intefaces;
using TomadaStore.ConsumerAPI.Services;
using TomadaStore.ConsumerAPI.Services.Intefaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<ConnectionDB>();

builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IConsumerService, ConsumerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
