using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using TestTasks.Data.Domains;
using TestTasks.Data.Domains.Models;

namespace TestN2
{
    internal class Program
    {
        private static string _getStr = "get";
        private static string _addStr = "add";
        private static string _exitStr = "exit";
        private static string _okStr = "[OK]";

        static readonly string WelcomeStr = $"{Environment.NewLine}Для ввода данных используйте команду {_addStr},{Environment.NewLine}Для поиска транзакции по Id используйте команду {_getStr},{Environment.NewLine}Для завершения работы программы используйте команду {_exitStr}{Environment.NewLine}";

        private static ILogger<TransactionDataProvider> _logger;
        private static TransactionDataProvider _dataProvider;

        static void Main(string[] args)
        {
            try
            {
                // Инициализируем DI и конфигурируем логгер
                var serviceProvider = new ServiceCollection()
                    .AddLogging(cfg => cfg.AddConsole())
                    .Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Information)
                    .AddTransient<TransactionDataProvider>()
                    .BuildServiceProvider();

                // Получаем инстанс логгера и присваеваем
                _logger = serviceProvider.GetService<ILogger<TransactionDataProvider>>();

                // Инициализируем класс провайдера данных
                _dataProvider = serviceProvider.GetRequiredService<TransactionDataProvider>();

                Console.WriteLine(WelcomeStr);

                string userInput;
                do
                {
                    userInput = UserInputData();

                    if (userInput.Equals(_addStr))
                    {
                        Add(userInput);
                    }
                    if (userInput.Equals(_getStr))
                    {
                        Find(userInput);
                    }

                } while (!userInput.Equals(_exitStr));

                if (userInput.Equals(_exitStr))
                    ExitApp();

                Console.ReadLine();
            }
            catch (Exception exc)
            {
                _logger.LogError($"{exc}");
            }

        }

        private static string UserInputData()
        {
            while (true)
            {
                var result = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(result))
                {
                    return result;
                }
                Console.WriteLine("Введите команду");
            }
        }

        private static void ExitApp()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Добавление транзакции 
        /// </summary>
        /// <param name="strLine"></param>
        private static void Add(string strLine)
        {
            if (strLine.ToLowerInvariant().Equals(_addStr))
            {
                Transaction tr = new Transaction();

                Console.WriteLine("Введите Id");

                if (int.TryParse(Console.ReadLine(), out int trId))
                {
                    tr.Id = trId;
                }
                else
                {
                    _logger.LogError($"Обнаружен некорректный ввод данных");
                }

                Console.WriteLine("Введите дату");

                if (DateTime.TryParse(Console.ReadLine(), out DateTime trDate))
                {
                    tr.TransactionDate = trDate;
                }
                else
                {
                    _logger.LogError($"Обнаружен некорректный ввод данных");
                }
                Console.WriteLine("Введите сумму");

                if (decimal.TryParse(Console.ReadLine(), out decimal trAmount))
                {
                    tr.Amount = trAmount;
                }
                else
                {
                    _logger.LogError($"Обнаружен некорректный ввод данных");
                }

                _dataProvider.Save(tr);

                Console.WriteLine(_okStr);

                Console.WriteLine("Введите команду");
            }

        }

        /// <summary>
        /// Поиск транзакции 
        /// </summary>
        /// <param name="strLine"></param>
        private static void Find(string strLine)
        {
            if (strLine.ToLowerInvariant().Equals(_getStr))
            {
                Console.WriteLine("Введите Id");

                if (int.TryParse(Console.ReadLine(), out int trId))
                {
                    string res = _dataProvider.FindByIdAsync(trId).Result;

                    if (!string.IsNullOrWhiteSpace(res))
                    {
                        Console.WriteLine($"{res}");
                        Console.WriteLine(_okStr);
                        Console.WriteLine("Введите команду");
                    }
                }
                else
                {
                    _logger.LogError($"Обнаружен некорректный ввод данных");
                }

            }

        }       
    }
}
