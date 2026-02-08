using Microsoft.Extensions.Logging;
using OrderProcessing.Domain.Dtos;
using OrderProcessing.Domain.Events;
using OrderProcessing.Persistence.Interfaces;
using OrderProcessing.Persistence.Models;

namespace OrderProcessing.Domain.Interfaces;

public class OrderProcessor : IOrderProcessor
{
    private readonly ILogger<OrdersManager> _logger;
    private readonly IOrdersRepository _ordersRepository;

    public OrderProcessor(ILogger<OrdersManager> logger, IOrdersRepository ordersRepository)
    {
        _logger = logger;
        _ordersRepository = ordersRepository;
    }

    public async Task ProcessOrder(OrderEventDto orderEvent)
    {
        if (orderEvent.Type != EventType.Added)
        {
            return;
        }

        var order = await _ordersRepository.Get(orderEvent.Id);
        if (order.Status == OrderStatus.Processed)
        {
            return;
        }

        // processing
        await Task.Delay(2000);

        order.DateProcessed = DateTime.UtcNow;
        order.Status = OrderStatus.Processed;

        await _ordersRepository.SaveChanges();

        _logger.LogInformation($"Order:{order.Id} processed.");
    }
}