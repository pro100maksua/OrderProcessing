using OrderProcessing.Domain.Dtos;
using OrderProcessing.Persistence.Models;

namespace OrderProcessing.Domain.Extensions
{
    public static class OrderExtensions
    {
        public static bool Validate(this CreateOrderDto dto)
        {
            if (dto.CustomerId == Guid.Empty)
            {
                return false;
            }

            if (dto.TotalAmount <= 0)
            {
                return false;
            }

            if (dto.Items?.Any() != true)
            {
                return false;
            }

            if (dto.Items.Any(i => i.Quantity * i.Amount != i.TotalAmount))
            {
                return false;
            }

            if (dto.Items.Sum(i => i.TotalAmount) != dto.TotalAmount)
            {
                return false;
            }

            return true;
        }

        public static Order ToEntity(this CreateOrderDto dto)
        {
            if (dto is null)
            {
                return null;
            }

            return new Order
            {
                CustomerId = dto.CustomerId,
                TotalAmount = dto.TotalAmount,
                Status = OrderStatus.Created,
                Items = dto.Items.Select(i => new OrderItem
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity,
                    Amount = i.Amount,
                    TotalAmount = i.TotalAmount,
                }).ToList(),
            };
        }
    }
}
