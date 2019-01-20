using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using DataImmune.Controllers;

namespace DataImmune
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        public string BaseOctavePath { get; set; }
        public string FullOctavePath { get; set; }
        public string Filename { get; set; }

        //Temporary startup until the real one is used.
        public Startup(string baseOctavePath, string fullOctavePath, string filename)
        {
            if (baseOctavePath != null && fullOctavePath != null)
            {
                BaseOctavePath = baseOctavePath;
                FullOctavePath = BaseOctavePath + fullOctavePath;
                Filename = filename;
            }
        }
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
            Initialize();
        }
        //public Startup(IHostingEnvironment env)
        //{
        //    var localBuilder = new ConfigurationBuilder()
        //   .SetBasePath(env.ContentRootPath)
        //   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //   .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

        //    var localSettings = localBuilder.Build();

        //    var builder = new ConfigurationBuilder()
        //    .SetBasePath(env.ContentRootPath)
        //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
        //    .AddDapperConfiguration(localSettings.GetConnectionString("DefaultConnection"));

        //    Configuration = builder.Build();

        //    var loggerConfig = new LoggerConfiguration()
        //                            .MinimumLevel.Information()
        //                            .WriteTo.RollingFile($"{env.ContentRootPath}\\log.txt", retainedFileCountLimit: 7, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{FOO}] [{Level}][{Host}] {Message}}{NewLine}{Exception}")
        //                           ;

        //    Log.Logger = loggerConfig.CreateLogger();
        //}

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddSingleton<IConfigurationRoot>(Configuration);

            services.AddOptions();
            services.Configure<ConnectionStringList>(Configuration.GetSection("ConnectionStrings"));

            //services.AddTransient(OctaveController,);
        }
        public void Initialize()
        {
            //Console.WriteLine("Opening Octave; please wait...");
            //OctaveController octave = new OctaveController();
            //string errorMessage = octave.StartOctave(FullOctavePath, Filename, false);
            //if (errorMessage != "")
            //{
            //    Console.WriteLine(errorMessage);
            //    Console.ReadLine();
            //    Environment.Exit(0);
            //}
            //Console.WriteLine("Octave successfully opened.");

            writeIntroText();

            //double[][] test_var = octave.GetMatrix(choice);
            //Console.WriteLine(test_var);
        }

        public static void writeIntroText()
        {
            string welcomeMessage = "Welcome to the E2Open Data Immune System Interface";
            //Console.WriteLine("\n");
            bannerText(welcomeMessage);
            Console.WriteLine(welcomeMessage);
            bannerText(welcomeMessage);
            Console.WriteLine("\n");
            Console.WriteLine("Choose an option below");
        }

        public static void vaccinateDatabase()
        {
            //Discover tables

            //Put list of tables in list

            // Create Dictionary of all columns in each table as Key with datatype as the Value

            //Pull our vaccine from the file

            //Example: vaccine is apostrophe; therefore, the context is only nvarchar() datatype columns

            //Run vaccine as transaction; for all exceptions, write to file

            //Output: show file to display the erroneous inerts
        }



        public static void bannerText(string baseText)
        {
            string banner = "*";
            for (int i = 0; i < baseText.Length; i++)
            {
                banner = banner + "*";
            }
            Console.WriteLine(banner);
            banner = "";
        }
    }
}
