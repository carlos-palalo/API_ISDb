using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using System;
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
            // Configuro que se pueda acceder al archivo appsettings.json
            IConfiguration Configuration;
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            // Configuro el logger para poder usarlo en toda la aplicación sin volver a instanciarlo
            _log = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            try
            {
                // Lanzo la aplicación
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

        /// <summary>
        /// Cada x tiempo (especificado en appsettings.json) se comprimen los archivos en Logs y se meten en app.log
        /// </summary>
        public static ArchiveHooks MyArchiveHooks => new ArchiveHooks(CompressionLevel.Fastest, "./Logs/app.log");
    }
}
