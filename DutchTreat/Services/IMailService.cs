using System.Threading.Tasks;

namespace DutchTreat.Services
{
    public interface IMailService
    {
        Task Send(string to, string subject, string body);
    }
}