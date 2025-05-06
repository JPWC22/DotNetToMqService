using Microsoft.AspNetCore.Mvc;
using DotNetToMQService.Services;

namespace DotNetToMQService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // HTTP POST endpoint: /api/message/sendToMQ
        [HttpPost("sendToMQ")]
        public IActionResult SendToMQ()
        {
            try
            {
                var message = _messageService.GenerateMessage();
                // Later, you'll send the generated message to RabbitMQ here!
                return Ok(new
                {
                    Message = "Message generated successfully.",
                    Data = message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}