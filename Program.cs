using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Serilog;
using Serilog.Core;
using System.IO;
using Serilog.Sinks.File.Archive;
using System.IO.Compression;
namespace API_ISDb
{
    public class Program
    {
        public static Logger _log;
        //public static IConfiguration Configuration { get; set; }
        public static void Main(string[] args)
        {
            IConfiguration Configuration;
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            _log = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
                _log.Error("Msg: " + ex.Message + " StackTrace: " + ex.StackTrace);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static ArchiveHooks MyArchiveHooks => new ArchiveHooks(CompressionLevel.Fastest, "./Logs/app.log");
    }
}
