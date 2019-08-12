using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TestTasks.Data.Domains
{
    /// <summary>
    /// Имплементация интерфейса хранения данных
    /// </summary>
    public class FIleDataProvider : IDataProvider
    {
        private readonly string _dataConnectionString;
        private readonly ILogger<IDataProvider> _logger;
        
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="config">путь к файлу для хранения данных</param>
        public FIleDataProvider(ILogger<IDataProvider> logger, IConfiguration config)
        {
            _dataConnectionString = string.IsNullOrWhiteSpace(config.GetSection("filePath").Value) ? $"{Path.Combine(Directory.GetCurrentDirectory(), "data.dat")}" : config.GetSection("filePath").Value;
            
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// Создание записи для хранения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task CreateAsync<T>(T entity)
        {
            try
            {
                string json =  JsonConvert.SerializeObject(entity);
                await Task.Delay(1000, cancellationToken: CancellationToken.None);
                throw new NotImplementedException();
            }
            catch (Exception exc)
            {
                _logger.LogError($"{exc}");
                throw;
            }
        }
        /// <summary>
        /// Поиск по id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> FindByIdAsync<T>(string id)
        {
            try
            {
                await Task.Factory.StartNew(() =>
                {

                });
                throw new NotImplementedException();
            }
            catch (Exception exc)
            {
                _logger.LogError($"{exc}");
                throw;
            }
        }
    }
}
