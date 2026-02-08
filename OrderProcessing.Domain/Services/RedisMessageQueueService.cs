using System.Text.Json;
using StackExchange.Redis;

namespace OrderProcessing.Domain.Interfaces;

public class RedisMessageQueueService : IMessageQueueService
{
    private readonly IDatabase _database;

    public RedisMessageQueueService(IConnectionMultiplexer connection)
    {
        _database = connection.GetDatabase();
    }

    public async Task Add<T>(string key, T data)
    {
        await _database.StreamAddAsync(
            key,
            [
                new NameValueEntry("data", JsonSerializer.Serialize(data)),
            ]);
    }
}