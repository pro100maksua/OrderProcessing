namespace OrderProcessing.Persistence.Models;

public class OrderItem
{
    public Guid Id { get; set; }

    public Guid ItemId { get; set; }

    public int Quantity { get; set; }

    public decimal Amount { get; set; }

    public decimal TotalAmount { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}