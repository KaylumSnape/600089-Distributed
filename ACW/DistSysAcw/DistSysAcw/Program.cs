using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistSysAcw.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DistSysAcw
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create RSA Instance.
            RsaCryptography.GetRsaInstance();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
