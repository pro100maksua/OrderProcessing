using OrderProcessing.Persistence.Models;

namespace OrderProcessing.Persistence.Interfaces;

public interface IOrdersRepository
{
    Task<Order> Get(Guid id);
    void Add(Order order);

    Task SaveChanges();
}