using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SupportAgent.Api.Controllers
{
    [ApiController]
    [Route("api/agent")]
    public class AgentController : ControllerBase
    {
        private readonly SupportAgent _agent;

        public AgentController(SupportAgent agent)
        {
            _agent = agent;
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] AgentRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request?.Message))
                return BadRequest(new { error = "Message is required." });

            var response = await _agent.RunAsync(request.Message, cancellationToken);
            return Ok(new AgentResponseDto(response.Text));
        }

        [HttpGet("/health")]
        public IActionResult Health()
        {
            return Ok(new { status = "healthy" });
        }
    }

    public sealed record AgentRequest(string Message);
    public sealed record AgentResponseDto(string Response);
}
