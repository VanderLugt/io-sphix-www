using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sphix.Service.Logger;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sphix.Web
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context, ILoggerService Ilogger)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, Ilogger);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception, ILoggerService logger)
        {
            var response = context.Response;
            BaseCustomException customException = exception as BaseCustomException;
            Int32 ErrorCode = (int)HttpStatusCode.InternalServerError;
            // response.Redirect("/Home/Error");
            var file = new FileInfo(@"wwwroot\response.html");
            byte[] buffer;
            if (file.Exists)
            {
                response.StatusCode = ErrorCode;// (int)HttpStatusCode.OK;
                response.ContentType = "text/html";
                string readfile= await File.ReadAllTextAsync(file.FullName);
                readfile = readfile.Replace("{ErrorCode}", ErrorCode.ToString());
                readfile = readfile.Replace("{Message}", exception.Message);
                readfile = readfile.Replace("{Source}", exception.Source);
                readfile = readfile.Replace("{Detail}", exception.StackTrace);
                buffer = Encoding.ASCII.GetBytes(readfile); // File.ReadAllBytes(file.FullName);
                context.Response.ContentLength = buffer.Length;

                await logger.AddAsync(new Sphix.DataModels.Logger.LoggerDataModel
                {
                    ErrorCode=ErrorCode.ToString(),
                    Message=exception.Message,
                    Source=exception.Source,
                    Detail=exception.StackTrace
                });

                using (var stream = context.Response.Body)
                {
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                    await stream.FlushAsync();
                }
                readfile = null;
            }
            else
            {
                response.ContentType = "application/json";
                response.StatusCode = ErrorCode;
                await response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorResponse
                {
                    Message = exception.Message,
                    Description ="error code"+ ErrorCode.ToString()
                }));
            }

        }

    }
    public class CustomErrorResponse
    {
        public string Message { get; set; }
        public string Description { get; set; }
    }
    public class BaseCustomException : Exception
    {
        private int _code;
        private string _description;

        public int Code
        {
            get => _code;
        }
        public string Description
        {
            get => _description;
        }
        public BaseCustomException(string message, string description, int code) : base(message)
        {
            _code = code;
            _description = description;
        }
    }
}
