using Microsoft.EntityFrameworkCore;

using StackExchange.Redis;

using OrderProcessing.Domain.Interfaces;
using OrderProcessing.Persistence;
using OrderProcessing.Persistence.Interfaces;
using OrderProcessing.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IOrdersManager, OrdersManager>();
builder.Services.AddTransient<IMessageQueueService, RedisMessageQueueService>();
builder.Services.AddTransient<IOrdersRepository, OrdersRepository>();

builder.Services.AddDbContext<OrderProcessingDbContext>(o =>
    o.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = ConfigurationOptions.Parse(
        builder.Configuration["Redis:ConnectionString"]!,
        true);

    config.AbortOnConnectFail = false;
    config.ConnectRetry = 5;
    config.ReconnectRetryPolicy = new ExponentialRetry(5000);

    return ConnectionMultiplexer.Connect(config);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderProcessingDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
