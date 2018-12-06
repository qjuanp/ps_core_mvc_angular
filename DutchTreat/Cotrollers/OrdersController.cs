using System;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrdersController : Controller
    {
        private readonly IDutchRepository _repository;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
            IDutchRepository repository,
            ILogger<OrdersController> logger
        )
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _repository.GetAllOrders());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders {ex}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var order = await _repository.GetOrderById(id);
                if (order != null) return Ok(order);
                else return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get orders {ex}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var order = new Order
                    {
                        Id = model.OrderId,
                        OrderDate = model.OrderDate == DateTime.MinValue ?
                            DateTime.Now : model.OrderDate,
                        OrderNumber = model.OrderNumber
                    };

                    await _repository.AddEntity(order);
                    if (await _repository.SaveAll())
                    {
                        var savedOrderViewModel = new OrderViewModel
                        {
                            OrderId = order.Id,
                            OrderNumber = order.OrderNumber,
                            OrderDate = order.OrderDate
                        };
                        return Created($"/api/Orders/{savedOrderViewModel.OrderId}", savedOrderViewModel);
                    }
                }
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save order {ex}");
                return BadRequest("Failed to save order");
            }

            return BadRequest("Not order saved");
        }
    }
}