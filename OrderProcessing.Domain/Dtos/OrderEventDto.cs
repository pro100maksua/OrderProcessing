using OrderProcessing.Domain.Events;

namespace OrderProcessing.Domain.Dtos;

public class OrderEventDto
{
    public Guid Id { get; set; }

    public EventType Type { get; set; }
}