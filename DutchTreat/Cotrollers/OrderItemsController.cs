using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/orders/{orderId:int}/items")]
    public class OrderItemsController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrderItemsController> _logger;
        private readonly IMapper _mapper;

        public OrderItemsController(
            IDutchRepository repository,
            ILogger<OrderItemsController> logger,
            IMapper mapper
        )
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int orderId)
        {
            var order = await _repository.GetOrderById(orderId);
            if (order != null) return Ok(_mapper.Map<IEnumerable<OrderItem>, IEnumerable<OrderItemViewModel>>(order.Items));
            else return NotFound("Order not found");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int orderId, int id)
        {
            var order = await _repository.GetOrderById(orderId);
            if (order != null)
            {
                var orderItem = order
                    .Items
                    .SingleOrDefault(o => o.Id == id);

                return Ok(_mapper.Map<OrderItem, OrderItemViewModel>(orderItem));
            }
            else return NotFound("Order not found");
        }
    }
}