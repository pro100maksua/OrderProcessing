using Microsoft.EntityFrameworkCore;
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

    public Task<Order?> Get(Guid id)
    {
        return _context.Orders.Include(e => e.Items)
            .FirstOrDefaultAsync(e => e.Id == id);
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