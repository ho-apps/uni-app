using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TestTasks.Data.Domains.Models;

namespace TestTasks.Data.Domains;

/// <summary>
///     Имплементация интерфейса хранения данных
/// </summary>
public class TransactionDataProvider
{
    private readonly ILogger<TransactionDataProvider> _logger;

    /// <summary>
    ///     Ctor
    /// </summary>
    /// <param name="logger"></param>
    public TransactionDataProvider(ILogger<TransactionDataProvider> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Transactions = new List<Transaction>();
    }

    private List<Transaction> Transactions { get; }

    /// <summary>
    ///     Создание записи во временном хранилище
    /// </summary>
    /// <param name="tr"></param>
    /// <returns></returns>
    public void Save(Transaction tr)
    {
        try
        {
            // Добавляем данные в коллекцию,
            // предварительно проверив на сущестовавание с транзакции с таким Id
            if (Transactions.Exists(a => a.Id == tr.Id)) throw new Exception("Запись с таким Id уже существует.");
            Transactions.Add(tr);
        }
        catch (Exception exc)
        {
            _logger.LogError($"{exc}");
            throw;
        }
    }

    /// <summary>
    ///     Поиск по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<string> FindByIdAsync(int id)
    {
        try
        {
            var result = string.Empty;

            await Task.Factory.StartNew(() =>
            {
                var tr = Transactions.Find(a => a.Id == id);

                result = tr == null ? $"Транзакция с Id {id} не существует." : JsonConvert.SerializeObject(tr);
            });

            return result;
        }
        catch (Exception exc)
        {
            _logger.LogError($"{exc}");
            throw;
        }
    }
}