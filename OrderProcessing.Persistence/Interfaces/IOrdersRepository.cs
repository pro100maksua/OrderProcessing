using OrderProcessing.Persistence.Models;

namespace OrderProcessing.Persistence.Interfaces;

public interface IOrdersRepository
{
    void Add(Order order);

    Task SaveChanges();
}