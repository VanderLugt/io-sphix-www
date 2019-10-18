using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Sphix.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
                WebHost.CreateDefaultBuilder(args)
                 .UseKestrel(options =>

                 {
                     options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(120);
                     options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(120);
                 })
                .UseStartup<Startup>();
    }
}
