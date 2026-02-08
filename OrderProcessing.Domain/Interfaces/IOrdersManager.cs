using OrderProcessing.Domain.Dtos;

namespace OrderProcessing.Domain.Interfaces;

public interface IOrdersManager
{
    Task<Response<Guid>> CreateOrder(CreateOrderDto dto);
}