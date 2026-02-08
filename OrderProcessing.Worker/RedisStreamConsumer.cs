using System.Text.Json;
using OrderProcessing.Domain.Dtos;
using OrderProcessing.Domain.Interfaces;
using StackExchange.Redis;

public class RedisStreamConsumer
{
    private readonly IOrderProcessor _orderProcessor;
    private readonly IDatabase _db;

    public RedisStreamConsumer(IConnectionMultiplexer redis, IOrderProcessor orderProcessor)
    {
        _orderProcessor = orderProcessor;
        _db = redis.GetDatabase();
    }

    public async Task ConsumeAsync(string stream, string group, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            var entries = await _db.StreamReadGroupAsync(
                stream, group, "worker", ">", count: 10);

            foreach (var entry in entries)
            {
                var data = entry.Values.First(v => v.Name == "data").Value;
                var orderEvent = JsonSerializer.Deserialize<OrderEventDto>(data);

                await _orderProcessor.ProcessOrder(orderEvent);

                await _db.StreamAcknowledgeAsync(
                    stream, group, entry.Id);
            }
        }
    }
}