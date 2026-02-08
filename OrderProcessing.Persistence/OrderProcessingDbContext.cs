using Microsoft.EntityFrameworkCore;

using OrderProcessing.Persistence.Models;

namespace OrderProcessing.Persistence
{
    public class OrderProcessingDbContext : DbContext
    {
        public OrderProcessingDbContext(DbContextOptions<OrderProcessingDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
