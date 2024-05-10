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

        [HttpGet]
        public async Task<IActionResult> GetOrder(int id)
        {
            if (!await _repository.DoesOrderExist(id))
            {
                return NotFound("Order not found");
            }

            var order = _repository.GetOrderInfo(id);
            
            return Ok(order);
        }
        
    }
}
