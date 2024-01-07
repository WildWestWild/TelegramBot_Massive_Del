using Microsoft.AspNetCore.Mvc;

namespace BotMassiveDel.Controllers
{
    public class HealthCheckController : Controller
    {
        [HttpGet("/HealthCheck/TryCheck")]
        public IActionResult TryCheck()
        {
            return Ok();
        }
    }
}
