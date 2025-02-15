using FraudDetection.Models;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace FraudDetection.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly RabbitMQService _rabbitMQService;

        public TransactionController(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        [HttpPost]
        public IActionResult PostTransaction([FromBody] TransactionDTO transaction)
        {
            var message = System.Text.Json.JsonSerializer.Serialize(transaction);
            _rabbitMQService.PublishMessage(message);
            return Ok(new {Message = "Transaction sent to queue"});
        }
    }
}
