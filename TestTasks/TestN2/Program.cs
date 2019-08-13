using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using TestTasks.Data.Domains;

namespace TestN2
{
    class Program
    {
        static void Main(string[] args)
        {
            // instantiate DI and configure logger
            var serviceProvider = new ServiceCollection()
                .AddLogging(cfg => cfg.AddConsole()).Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Information)
                .AddTransient<EntityDataProvider>()
                .BuildServiceProvider();

            // get instance of logger
            var logger = serviceProvider.GetService<ILogger<Program>>();

            logger.LogWarning($"Внимание! Запущен процесс...");

            
            logger.LogInformation($"Для завершения работы программы нажмите <Enter>...");

            Console.ReadLine();
        }
    }
}
