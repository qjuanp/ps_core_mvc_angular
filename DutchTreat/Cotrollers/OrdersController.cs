using System;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
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
        public async Task<IActionResult> Post([FromBody] Order model)
        {
            try
            {
                await _repository.AddEntity(model);
                if (await _repository.SaveAll())
                    return Created($"/api/Orders/{model.Id}", model);
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