using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AWSS3.Utility
{
  public class AWSS3Bucket: IAWSS3Bucket
    {
        private AwsS3Settings _awsS3Settings;
        RegionEndpoint endPoint;
        public AWSS3Bucket(IOptions<AwsS3Settings> optionsAccessor)
        {
            _awsS3Settings = optionsAccessor.Value;
            endPoint = RegionEndpoint.USWest1;
        }
        public async Task<bool> UploadFileAsync(string folderPath, IFormFile file,string fileName)
        {
            if (file == null)
                return false;
            if(string.IsNullOrEmpty(fileName))
            {
                fileName = file.Name;
            }
            if (await CreateFolder(folderPath)) {
                try
                {
                    using (var client = new AmazonS3Client(_awsS3Settings.AwsAccessKeyId, _awsS3Settings.AwsSecretAccessKey, endPoint))
                    {
                        using (var newMemoryStream = new MemoryStream())
                        {
                            file.CopyTo(newMemoryStream);
                            var uploadRequest = new TransferUtilityUploadRequest
                            {
                                InputStream = newMemoryStream,
                                Key = fileName,
                                BucketName = _awsS3Settings.BucketName+"/"+folderPath,
                                CannedACL = S3CannedACL.PublicRead
                            };
                            var fileTransferUtility = new TransferUtility(client);
                            await fileTransferUtility.UploadAsync(uploadRequest);
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    //return false;
                    throw;
                }
            }
            return false;
        }
        public async Task<bool> UploadFileAsync(string folderPath, IFormFile file)
        {
            if (file == null)
                return false;
          
            if (await CreateFolder(folderPath))
            {
                try
                {
                    using (var client = new AmazonS3Client(_awsS3Settings.AwsAccessKeyId, _awsS3Settings.AwsSecretAccessKey, endPoint))
                    {
                        using (var newMemoryStream = new MemoryStream())
                        {
                            file.CopyTo(newMemoryStream);
                            var uploadRequest = new TransferUtilityUploadRequest
                            {
                                InputStream = newMemoryStream,
                                Key = file.FileName,
                                BucketName = _awsS3Settings.BucketName + "/" + folderPath,
                                CannedACL = S3CannedACL.PublicRead
                            };
                            var fileTransferUtility = new TransferUtility(client);
                            await fileTransferUtility.UploadAsync(uploadRequest);
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    //return false;
                    throw;
                }
            }
            return false;
        }
        public async Task<bool> DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            string folderPath = string.Empty;
            string fileName = string.Empty;
            folderPath = filePath.Remove(filePath.LastIndexOf('/'), (filePath.Length - filePath.LastIndexOf('/')));
            fileName = filePath.Remove(0, filePath.LastIndexOf('/') + 1);
            try
                {
                    using (var client = new AmazonS3Client(_awsS3Settings.AwsAccessKeyId, _awsS3Settings.AwsSecretAccessKey, endPoint))
                    {
                        var deleteObjectRequest = new DeleteObjectRequest
                        {
                            BucketName = _awsS3Settings.BucketName + "/" + folderPath,
                            Key = fileName
                        };
                    //    Console.WriteLine("Deleting an object");
                        await client.DeleteObjectAsync(deleteObjectRequest);
                    }
                return true;
                }
                catch (Exception)
                {
                    //return false;
                    throw;
                }
        }
        private async Task<bool> CreateFolder(string folderPath)
        {
            try
            {
                using (var client = new AmazonS3Client(_awsS3Settings.AwsAccessKeyId, _awsS3Settings.AwsSecretAccessKey, endPoint))
                {
                    using (var newMemoryStream = new MemoryStream())
                    {
                        var key = string.Format(@"{0}/", folderPath);
                        var uploadRequest = new TransferUtilityUploadRequest
                        {
                            InputStream = newMemoryStream,
                            Key = key,
                            BucketName = _awsS3Settings.BucketName,
                            CannedACL = S3CannedACL.PublicRead
                        };
                        var fileTransferUtility = new TransferUtility(client);
                        await fileTransferUtility.UploadAsync(uploadRequest);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                //return false;
                return false;
            }
            
        }
      
    }
}
