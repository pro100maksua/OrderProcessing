using Microsoft.Extensions.Logging;
using OrderProcessing.Domain.Dtos;
using OrderProcessing.Domain.Events;
using OrderProcessing.Domain.Extensions;
using OrderProcessing.Persistence.Interfaces;

namespace OrderProcessing.Domain.Interfaces;

public class OrdersManager : IOrdersManager
{
    private readonly ILogger<OrdersManager> _logger;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IMessageQueueService _messageQueueService;

    public OrdersManager(ILogger<OrdersManager> logger, IOrdersRepository ordersRepository, IMessageQueueService messageQueueService)
    {
        _logger = logger;
        _ordersRepository = ordersRepository;
        _messageQueueService = messageQueueService;
    }

    public async Task<Response<Guid>> CreateOrder(CreateOrderDto dto)
    {
        try
        {
            var isValid = dto.Validate();
            if (!isValid)
            {
                return new Response<Guid>(errorCode: 1, "Bad request");
            }

            var order = dto.ToEntity();
            _ordersRepository.Add(order);
            await _ordersRepository.SaveChanges();

            var orderEvent = new OrderEventDto
            {
                Id = order.Id,
                Type = EventType.Added
            };
            await _messageQueueService.Add("orders", orderEvent);

            return new Response<Guid>(order.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message, new { dto.CustomerId });
            return new Response<Guid>(errorCode: 2, "Internal Error");
        }
    }
}