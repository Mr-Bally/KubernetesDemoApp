using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KubernetesDemoApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public HttpStatusCode Get()
        {
            _logger.LogInformation("Test endpoint hit");

            return HttpStatusCode.OK;
        }
    }
}