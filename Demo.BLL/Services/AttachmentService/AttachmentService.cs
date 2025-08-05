using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        List<string> allowedExtensions =  [ ".png", ".jpg", ".Jepg" ];
        const int maxSize = 2_097_152;
        public string? Upload(IFormFile file, string folderName)
        {
            // 1.Check Extension
            var extension = Path.GetExtension(file.FileName); // .png
            if (!allowedExtensions.Contains(extension)) return null;

            // 2.Check Size
            if(file.Length == 0  || file.Length > maxSize) return null;

            // 3.Get Located Folder Path
            //var folderPath = "E:\\Route\\C43\\MVC\\Demo\\MVCDemo\\Demo.PL\\wwwroot\\Files\\images\\" ;// local invalid
            //var folderPath = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Files\\{folderName}";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

            // 4.Make Attachment Name Unique-- GUID
            var fileName = $"{Guid.NewGuid()}_{file.FileName}"; // Unique

            // 5.Get File Path
            var filePath = Path.Combine(folderPath, fileName);

            // 6.Create File Stream To Copy File[Unmanaged]
            using FileStream fs = new FileStream(filePath, FileMode.Create);
            // 7.Use Stream To Copy File
            file.CopyTo(fs);

            // 8.Return FileName To Store In Database
            return fileName;
        }
        public bool Delete(string fileName , string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName, fileName);
            if(!File.Exists(filePath)) return false;
            else
            {
                File.Delete(filePath);
                return true;
            }
        }


    }
}
