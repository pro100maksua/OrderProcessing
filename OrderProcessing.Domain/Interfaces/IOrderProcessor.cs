using OrderProcessing.Domain.Dtos;

namespace OrderProcessing.Domain.Interfaces;

public interface IOrderProcessor
{
    Task ProcessOrder(OrderEventDto orderEvent);
}