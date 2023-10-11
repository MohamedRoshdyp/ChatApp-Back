using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Persistence.Helpers;
public static class PhotoManager
{
    //updload
    public static string _uploadPhoto(this IWebHostEnvironment webHost,IFormFile file,string pathName)
    {
        string src = "";
        string root = "wwwroot/";
        if(!Directory.Exists(root + $"Images/{pathName}"))
        {
            Directory.CreateDirectory(root + $"Images/{pathName}");
        }
        if (file is not null)
        {
            src = $"Images/{pathName}/" + Guid.NewGuid() + file.FileName;
            string path = Path.Combine(webHost.ContentRootPath, root, src);

            using (var fileStream = new FileStream(path,FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }
        return src;
    }


    //remove
    public static void _removePhoto(this IWebHostEnvironment webHost,string oldFileName)
    {
        if (!string.IsNullOrEmpty(oldFileName))
        {
            string root = "wwwroot/";
            string oldPath = Path.Combine(webHost.ContentRootPath, root, oldFileName);
            File.Delete(oldPath);
        }
    }
}
