using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GYM.BLL.AttachementService
{
    public interface IAttachementService
    {
        string? Upload(string FolderName, IFormFile file);
        bool Delete(string FolderName, string FileName);
    }
}
