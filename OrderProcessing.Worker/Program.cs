using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderProcessing.Domain.Interfaces;
using OrderProcessing.Persistence;
using OrderProcessing.Persistence.Interfaces;
using OrderProcessing.Persistence.Repositories;
using StackExchange.Redis;

var services = new ServiceCollection();

services.AddLogging(b =>
{
    b.SetMinimumLevel(LogLevel.Information);
});

services.AddSingleton<IConfiguration>(sp =>
    new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false)
        .AddEnvironmentVariables()
        .Build());

services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var options = ConfigurationOptions.Parse(
        config["Redis:ConnectionString"]!, true);

    options.AbortOnConnectFail = false;
    return ConnectionMultiplexer.Connect(options);
});

services.AddTransient<IOrderProcessor, OrderProcessor>();
services.AddTransient<IMessageQueueService, RedisMessageQueueService>();
services.AddTransient<IOrdersRepository, OrdersRepository>();
services.AddSingleton<RedisStreamConsumer>();

services.AddDbContext<OrderProcessingDbContext>((sp, o) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    o.UseNpgsql(config.GetConnectionString("Database"));
});

services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var redisConfig = ConfigurationOptions.Parse(
        configuration["Redis:ConnectionString"]!,
        true);

    redisConfig.AbortOnConnectFail = false;
    redisConfig.ConnectRetry = 5;
    redisConfig.ReconnectRetryPolicy = new ExponentialRetry(5000);

    return ConnectionMultiplexer.Connect(redisConfig);
});

var provider = services.BuildServiceProvider();

var logger = provider.GetRequiredService<ILogger<Program>>();
var consumer = provider.GetRequiredService<RedisStreamConsumer>();

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
    logger.LogInformation("Shutdown requested...");
};

// bootstrap group once
await RedisBootstrap.EnsureGroupAsync(
    provider.GetRequiredService<IConnectionMultiplexer>(),
    "orders",
    "order-processors");

// main loop
logger.LogInformation("Redis worker started");

await consumer.ConsumeAsync(
    "orders",
    "order-processors",
    cts.Token);

logger.LogInformation("Redis worker stopped");