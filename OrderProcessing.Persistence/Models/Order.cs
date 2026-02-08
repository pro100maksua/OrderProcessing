namespace OrderProcessing.Persistence.Models;

public class Order
{
    public Guid Id { get; set; }

    public Guid CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; }

    public ICollection<OrderItem> Items { get; set; }
}