using OrderProcessing.Domain.Dtos;
using OrderProcessing.Domain.Extensions;
using OrderProcessing.Persistence.Interfaces;

namespace OrderProcessing.Domain.Interfaces;

public class OrdersManager : IOrdersManager
{
    private readonly IOrdersRepository _ordersRepository;

    public OrdersManager(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task<Response<Guid>> CreateOrder(CreateOrderDto dto)
    {
        var isValid = dto.Validate();
        if (!isValid)
        {
            return new Response<Guid>(errorCode: 1, "Bad request");
        }

        var order = dto.ToEntity();
        _ordersRepository.Add(order);
        await _ordersRepository.SaveChanges();

        return new Response<Guid>(order.Id);
    }
}