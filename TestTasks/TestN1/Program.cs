﻿using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TestN1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((builder, config) =>
                {
                    IConfigurationRoot buildConfig = config.Build();
                    ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

                    configurationBuilder.AddEnvironmentVariables();
                    config.AddJsonFile(Path.Combine("appsettings.json"), false, true);
                    config.AddJsonFile(Path.Combine("appsettings.Development.json"), true, true);
                    config.AddConfiguration(configurationBuilder.Build());
                }).ConfigureLogging((hostingContext, builder) =>
                {
                    builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    builder.AddConsole();
                    builder.AddDebug();
                });
    }
}
