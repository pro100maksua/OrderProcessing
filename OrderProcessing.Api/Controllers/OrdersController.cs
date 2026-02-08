using Microsoft.AspNetCore.Mvc;

using OrderProcessing.Domain.Dtos;
using OrderProcessing.Domain.Interfaces;

namespace OrderProcessing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersManager _ordersManager;

        public OrdersController(IOrdersManager ordersManager)
        {
            _ordersManager = ordersManager;
        }

        [HttpPost]
        public Task<Response<Guid>> CreateOrder(CreateOrderDto dto)
        {
            return _ordersManager.CreateOrder(dto);
        }
    }
}
