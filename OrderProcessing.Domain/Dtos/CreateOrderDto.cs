namespace OrderProcessing.Domain.Dtos;

public class CreateOrderDto
{
    public Guid CustomerId { get; set; }

    public decimal TotalAmount { get; set; }

    public List<CreateOrderItemDto> Items { get; set; }
}