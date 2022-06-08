using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Services
{
    public class UploadService : IUploadService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> UploadFile(string folder, IFormFile file)
        {

            folder += System.Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverPath = Path.Combine(_webHostEnvironment.WebRootPath, folder);
            var fileStream = new FileStream(serverPath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            await fileStream.DisposeAsync();

            return "/" + folder;
        }

        public void DeleteFile(string url)
        {
            if (File.Exists(_webHostEnvironment.WebRootPath + url))
                File.Delete(_webHostEnvironment.WebRootPath + url);
        }
    }
}
