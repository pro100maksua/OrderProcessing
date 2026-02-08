using OrderProcessing.Persistence.Interfaces;
using OrderProcessing.Persistence.Models;

namespace OrderProcessing.Persistence.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly OrderProcessingDbContext _context;

    public OrdersRepository(OrderProcessingDbContext context)
    {
        _context = context;
    }

    public void Add(Order order)
    {
        _context.Add(order);
    }

    public Task SaveChanges()
    {
        return _context.SaveChangesAsync();
    }
}