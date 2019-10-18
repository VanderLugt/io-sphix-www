using Amazon;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AWSS3.Utility
{
   public interface IAWSS3Bucket
    {
        Task<bool> UploadFileAsync(string folderPath, IFormFile file, string fileName);
        Task<bool> UploadFileAsync(string folderPath, IFormFile file);
        Task<bool> DeleteFileAsync(string filePath);
    }
}
