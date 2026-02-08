namespace OrderProcessing.Domain.Interfaces;

public interface IMessageQueueService
{
    Task Add<T>(string key, T data);
}