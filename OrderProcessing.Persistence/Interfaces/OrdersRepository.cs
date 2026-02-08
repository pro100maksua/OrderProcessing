using OrderProcessing.Persistence.Models;

namespace OrderProcessing.Persistence.Interfaces;

public class OrdersRepository : IOrdersRepository
{
    public void Add(Order order)
    {
        throw new NotImplementedException();
    }

    public Task SaveChanges()
    {
        throw new NotImplementedException();
    }
}