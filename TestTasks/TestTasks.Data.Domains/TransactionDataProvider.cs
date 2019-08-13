using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestTasks.Data.Domains.Models;

namespace TestTasks.Data.Domains
{
    /// <summary>
    /// Имплементация интерфейса хранения данных
    /// </summary>
    public class TransactionDataProvider
    {
        private readonly ILogger<TransactionDataProvider> _logger;

        private List<Transaction> Transactions { get; set; }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        public TransactionDataProvider(ILogger<TransactionDataProvider> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Transactions = new List<Transaction>();
        }

        /// <summary>
        /// Создание записи во временном хранилище
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task CreateAsync(string data)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    // Десериализуем строку в обьект
                    object obj = JsonConvert.DeserializeObject(data);

                    // Сравним десериализованный тип с моделями
                    // если тип соответствует, пытаемся записать в коллекцию
                    if (obj.GetType() == typeof(Transaction))
                    {
                        Transaction tr = (Transaction)obj;

                        if (Transactions.Exists(a => a.Id == tr.Id))
                        {
                            throw new Exception($"Запись с таким Id уже существует.");
                        }

                        Transactions.Add(tr);
                    }
                    else
                    {
                        throw new Exception($"Объект типа {obj.GetType()} с таким Id уже существует.");
                    }

                });

            }
            catch (Exception exc)
            {
                _logger.LogError($"{exc}");
                throw;
            }
        }
        /// <summary>
        /// Поиск по id
        /// </summary>/
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Transaction> FindByIdAsync(int id)
        {
            Transaction result = default(Transaction);
            try
            {
                if (Transactions == null)
                {
                    throw new Exception($"Хранилище пусто.");
                }

                await Task.Factory.StartNew(() =>
                {
                    Transaction tr = Transactions.Find(a => a.Id == id);

                    result = tr ?? throw new Exception($"Транзакция с Id {id} не существует.");
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
}
