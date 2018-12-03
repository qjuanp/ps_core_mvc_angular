using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DutchTreat.Services
{
    class NullMailService : IMailService
    {
        private readonly ILogger<NullMailService> _logger;

        public NullMailService(ILogger<NullMailService> logger) =>
            _logger = logger;

        public Task Send(string to, string subject, string body)
        {
            _logger.LogInformation($"To: {to} | Subject:{subject} | Body:{body}");

            return Task.CompletedTask;
        }
    }
}