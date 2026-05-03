using Inventory.Application.Features.Stock.CreateStock;
using Inventory.Application.Interfaces;
using Inventory.Application.Services;
using Inventory.Infrastructure.Configuration;
using Inventory.Infrastructure.Messaging.Consumers;
using Inventory.Infrastructure.Persistence.Mongo;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MongoDbContext>();

builder.Services.Configure<HostOptions>(options =>
{
    options.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var context = sp.GetRequiredService<MongoDbContext>();
    return context.GetType().GetField("_database", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(context) as IMongoDatabase;
});
BsonSerializer.RegisterSerializer(
    new GuidSerializer(GuidRepresentation.Standard));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateStockHandler).Assembly));
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddHostedService<ProductCreatedConsumer>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
