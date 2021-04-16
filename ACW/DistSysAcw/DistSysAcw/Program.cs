using DistSysAcw.Cryptography;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DistSysAcw
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Create RSA singleton Instance.
            RsaCryptography.GetRsaInstance();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}