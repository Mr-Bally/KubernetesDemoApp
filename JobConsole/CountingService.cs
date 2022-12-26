using Microsoft.Extensions.Logging;

namespace JobConsole
{
    public class CountingService
    {
        private readonly ILogger<CountingService> _logger;

        public CountingService()
        {
            var factory = new LoggerFactory();
            _logger = factory.CreateLogger<CountingService>();
        }

        public void StartCounting()
        {
            _logger.LogInformation("Starting the count");

            for (var i = 0; i < 100; i++)
            {
                Console.WriteLine(i.ToString());
                Thread.Sleep(1000);
            }

            _logger.LogInformation("Finished the count");
        }
    }
}
