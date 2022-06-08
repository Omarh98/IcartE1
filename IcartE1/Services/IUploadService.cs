using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace IcartE1.Services
{
    public interface IUploadService
    {
        Task<string> UploadFile(string folder, IFormFile file);
        void DeleteFile(string url);
    }
}