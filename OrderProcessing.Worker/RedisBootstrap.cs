using StackExchange.Redis;

public static class RedisBootstrap
{
    public static async Task EnsureGroupAsync(
        IConnectionMultiplexer redis,
        string stream,
        string group)
    {
        var db = redis.GetDatabase();

        try
        {
            await db.StreamCreateConsumerGroupAsync(
                stream,
                group,
                "$",              // start from new messages
                createStream: true);
        }
        catch (RedisServerException ex)
            when (ex.Message.Contains("BUSYGROUP"))
        {
            // Group already exists → safe to ignore
        }
    }
}