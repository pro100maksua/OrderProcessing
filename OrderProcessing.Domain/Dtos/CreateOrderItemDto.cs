namespace OrderProcessing.Domain.Dtos;

public class CreateOrderItemDto
{
    public Guid ItemId { get; set; }

    public int Quantity { get; set; }

    public decimal Amount { get; set; }

    public decimal TotalAmount { get; set; }
}