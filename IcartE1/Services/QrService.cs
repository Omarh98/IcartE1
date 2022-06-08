using Microsoft.AspNetCore.Hosting;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IcartE1.Services
{
    public class QrService : IQrService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public QrService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> CreateQRCodeAsync(string QrText)
        {
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(QrText, QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);

            Bitmap QrBitmap = QrCode.GetGraphic(60);
            byte[] BitmapArray = BitmapToByteArray(QrBitmap);

            string fileName = Guid.NewGuid().ToString() + "_" + "batch.png";
            string serverPath = Path.Combine(webHostEnvironment.WebRootPath, "images/", fileName);
            await File.WriteAllBytesAsync(serverPath, BitmapArray);
            return "/" +"images/"+ fileName;
        }

        private static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            using MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
