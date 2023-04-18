using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestTasks.Data.Domains;
using TestTasks.Data.Domains.Models;

namespace TestN2;

internal class Program
{
    private const string GetStr = "get";
    private const string AddStr = "add";
    private const string ExitStr = "exit";
    private const string OkStr = "[OK]";

    private static readonly string WelcomeStr =
        $"{Environment.NewLine}Для ввода данных используйте команду {AddStr},{Environment.NewLine}Для поиска транзакции по Id используйте команду {GetStr},{Environment.NewLine}Для завершения работы программы используйте команду {ExitStr}{Environment.NewLine}";

    private static ILogger<TransactionDataProvider> _logger;
    private static TransactionDataProvider _dataProvider;

    // ReSharper disable once UnusedParameter.Local
    private static void Main(string[] args)
    {
        try
        {
            // Инициализируем DI и конфигурируем логгер
            var serviceProvider = new ServiceCollection()
                .AddLogging(cfg => cfg.AddConsole())
                .Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Information)
                .AddTransient<TransactionDataProvider>()
                .BuildServiceProvider();

            // Получаем экземпляр логгера и присваиваем
            _logger = serviceProvider.GetService<ILogger<TransactionDataProvider>>();

            // Инициализируем класс провайдера данных
            _dataProvider = serviceProvider.GetRequiredService<TransactionDataProvider>();

            Console.WriteLine(WelcomeStr);

            string userInput;
            do
            {
                userInput = UserInputData();

                switch (userInput)
                {
                    case AddStr:
                        Add(userInput);
                        break;
                    case GetStr:
                        Find(userInput);
                        break;
                }
            } while (!userInput.Equals(ExitStr));

            if (userInput.Equals(ExitStr))
                ExitApp();

            Console.ReadLine();
        }
        catch (Exception exc)
        {
            _logger.LogError($@"{exc}");
        }
    }

    private static string UserInputData()
    {
        while (true)
        {
            var result = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(result)) return result;
            Console.WriteLine("Введите команду");
        }
    }

    private static void ExitApp()
    {
        Environment.Exit(0);
    }

    /// <summary>
    ///     Добавление транзакции
    /// </summary>
    /// <param name="strLine"></param>
    private static void Add(string strLine)
    {
        if (!strLine.ToLowerInvariant().Equals(AddStr)) return;
        Transaction tr = new();

        Console.WriteLine("Введите Id");

        if (int.TryParse(Console.ReadLine(), out var trId))
            tr.Id = trId;
        else
            _logger.LogError("Обнаружен некорректный ввод данных");

        Console.WriteLine("Введите дату");

        if (DateTime.TryParse(Console.ReadLine(), out var trDate))
            tr.TransactionDate = trDate;
        else
            _logger.LogError("Обнаружен некорректный ввод данных");
        Console.WriteLine("Введите сумму");

        if (decimal.TryParse(Console.ReadLine(), out var trAmount))
            tr.Amount = trAmount;
        else
            _logger.LogError("Обнаружен некорректный ввод данных");

        _dataProvider.Save(tr);

        Console.WriteLine(OkStr);

        Console.WriteLine("Введите команду");
    }

    /// <summary>
    ///     Поиск транзакции
    /// </summary>
    /// <param name="strLine"></param>
    private static void Find(string strLine)
    {
        if (strLine.ToLowerInvariant().Equals(GetStr))
        {
            Console.WriteLine("Введите Id");

            if (int.TryParse(Console.ReadLine(), out var trId))
            {
                var res = _dataProvider.FindByIdAsync(trId).Result;

                if (!string.IsNullOrWhiteSpace(res))
                {
                    Console.WriteLine($"{res}");
                    Console.WriteLine(OkStr);
                    Console.WriteLine("Введите команду");
                }
            }
            else
            {
                _logger.LogError("Обнаружен некорректный ввод данных");
            }
        }
    }
}