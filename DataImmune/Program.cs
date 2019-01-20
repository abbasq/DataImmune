using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tools;
using DataImmune.Controllers;
using DataImmune.Models;

namespace DataImmune
{
    class Program
    {
        private static string dataConnectionString { get; set; }
        static void Main(string[] args)
        {
            ErrorMessages em = new ErrorMessages();
            //Startup startup = new Startup("C:\\Source\\", "DataImmune.Program\\Octave_bin", "octave-cli-4.4.1.exe");
            //startup.Initialize();

            IServiceCollection services = new ServiceCollection();

            Startup startup = new Startup();
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            //configure console logging
            serviceProvider
                .GetService<ILoggerFactory>()
                .AddConsole(LogLevel.Debug);

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogDebug("Logger is working!");



            dataConnectionString = startup.Configuration.GetConnectionString("DefaultConnection1");
            if (dataConnectionString == null)
            {
                Console.WriteLine(em.UnableToConnectToDatabase());
            }

            BasicDBOperations dbOps = new BasicDBOperations(dataConnectionString);
            Application app = new Application();
            List<Application> allapplications = dbOps.GetApplications();

            //foreach (var item in allapplications)
            //{
            //    Console.WriteLine("Name: " + item.Name + "; Connection string: " + item.DbConnectionString);
            //}

            AppText appText = new AppText();
            List<AppText> appTextList = dbOps.GetTextByKey(1, "STARTUP");

            int i = 1;
            foreach (var item in appTextList)
            {
                if (item.Subkey != "WelcomeMessage")
                {
                    Console.WriteLine("  " + i + ". " + item.Text);
                    i++;
                }                
            }

            string choice = stringInput();
            switch (choice)
            {
                case "1":
                    break;
                case "2":                    
                    break;
                case "3":
                    Console.WriteLine("\nAvailable Databases:");
                    Console.WriteLine("\nNAME");
                    if (allapplications != null)
                    {
                        foreach (var appItem in allapplications)
                        {
                            Console.WriteLine(appItem.Name);
                        }
                    }

                    Console.Write("\nPlease specify database name: ");
                    string dbName = Console.ReadLine();
                    Console.WriteLine("\n");
                    Application selectedDb;
                    foreach (var item in allapplications)
                    {
                        if (item.Name == dbName)
                        {
                            selectedDb = item;
                            DatabaseInvestigator dbInvestigator = new DatabaseInvestigator(selectedDb.DbConnectionString);

                            Console.WriteLine("Table List for " + selectedDb.Name + ":");                            
                            List<Table> TableList = dbInvestigator.PeekDataBase(selectedDb.Name);
                            int spaceAmount = selectedDb.Name.Length;
                            int headerSpace = ("TABLE_CATALOG".Length);
                            int columnDelimiter = 3;
                            if (spaceAmount < headerSpace)
                            {
                                spaceAmount = columnDelimiter;
                            }
                            else
                            {
                                spaceAmount = (spaceAmount - headerSpace) + columnDelimiter;
                            }
                            string spaceString = "   "; //Start off with a certain amount spaces as a delimiter between columns
                            for (int j = 0; j < spaceAmount; j++)
                            {
                                spaceString = spaceString + " ";
                            }
                            Console.WriteLine("\nTABLE_CATALOG" + spaceString + "TABLE_NAME");
                            foreach (var tableItem in TableList)
                            {
                                Console.WriteLine(tableItem.TableCatalog + "   " + tableItem.TableName);
                            }
                            break;
                        }
                    }
                    Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Please select the number corresponding to one of the options above. (Note: as this is a work in progress your selection will exit the program.");
                    stringInput();
                    break;
            }



        }

        public static string stringInput()
        {
            return Console.ReadLine();
        }
    }
}
