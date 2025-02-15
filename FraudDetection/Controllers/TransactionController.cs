using FraudDetection.Models;
using FraudDetection.Services;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace FraudDetection.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly RabbitMQService _rabbitMQService;
        private readonly FraudDetectionService _fraudDetectionService;

        public TransactionController(RabbitMQService rabbitMQService, FraudDetectionService fraudDetectionService)
        {
            _rabbitMQService = rabbitMQService;
            _fraudDetectionService = fraudDetectionService;
        }

        [HttpPost]
        public IActionResult PostTransaction([FromBody] TransactionDTO transaction)
        {
            var fraudCheckResult = _fraudDetectionService.IsFraudulentTransaction(transaction);

            if(fraudCheckResult.Contains("risco de fraude"))
            {
                return BadRequest(new {Message = fraudCheckResult});
            }

            var message = System.Text.Json.JsonSerializer.Serialize(transaction);
            _rabbitMQService.PublishMessage(message);
            return Ok(new {Message = "Transaction sent to queue"});
        }
    }
}
