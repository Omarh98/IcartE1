using System.Threading.Tasks;

namespace IcartE1.Services
{
    public interface IQrService
    {
        Task<string> CreateQRCodeAsync(string QrText);
    }
}