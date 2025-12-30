using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



using Microsoft.AspNetCore.Http;

namespace GYM.BLL.AttachementService
{
    public class AttachementService : IAttachementService
    {
        
        private readonly string[] extensions = { ".jpg", ".png", ".jpeg" };
        private readonly int MaxSize = 5 * 1024 * 1024;
        public string? Upload(string FolderName, IFormFile file)
        {
            try
            {
                if (FolderName is null || file is null || file.Length == 0)
                    return null;
                if (file.Length > MaxSize) return null;

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!extensions.Contains(extension)) return null;

                var folderpath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "images", FolderName);
                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(folderpath, fileName);
                using var filestream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(filestream);

                return fileName;
            }catch(Exception ex)
            {
                Console.WriteLine($"Cant Create file : {ex}");
                return null;
            }
         


        }
        public bool Delete(string FolderName, string FileName)
        {
            try
            {
                if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(FolderName)) return false;

                var path = Path.Combine(Directory.GetCurrentDirectory(), "images", FolderName, FileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }

                return false;
            }catch (Exception ex)
            {
                Console.WriteLine($"Cant Delete file : {ex}");
                return false;
            }
           
        }

    }
}
