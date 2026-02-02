using System.Threading.Tasks;

namespace inzibackend.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}