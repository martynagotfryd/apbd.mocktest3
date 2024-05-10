using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using mocktest3.Repositories;

namespace mocktest3.Controllers
{
    [Route("api/shop")]
    [Host]
    public class MyController : ControllerBase
    {
        private readonly IRepository _repository;

        public MyController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            if (!await _repository.DoesOrderExist(id))
            {
                return NotFound("Order not found");
            }

            var order = await _repository.GetOrderInfo(id);
            
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(int orderId, int productId, int amount)
        {
            if (!await _repository.DoesOrderExist(orderId))
            {
                return NotFound("Order not found");
            }
            if (!await _repository.DoesProductExist(productId))
            {
                return NotFound("Product not found in given order");
            }

            await _repository.UpdateAmount(orderId, productId, amount);

            return Ok();
        }
    }
}
