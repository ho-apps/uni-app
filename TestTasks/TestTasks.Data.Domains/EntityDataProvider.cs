using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestTasks.Data.Domains.Models;

namespace TestTasks.Data.Domains
{
    public class EntityDataProvider
    {
        private readonly ILogger<EntityDataProvider> _logger;
        private List<Entity> Entities { get; set; }

        public EntityDataProvider(ILogger<EntityDataProvider> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Entities = new();
        }
        /// <summary>
        /// Создание записи в хранилище данных
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
                        Entity obj = JsonConvert.DeserializeObject<Entity>(data);

                        // если запись с таким Id уже существует, выбрасываем исключение
                        if (Entities.Exists(a => a.Id == obj.Id))
                        {
                            throw new($"Запись с таким Id уже существует.");
                        }

                        // иначе записываем во временное хранилище
                        Entities.Add(obj);

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
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Entity> FindByIdAsync(Guid id)
        {
            try
            {
                Entity result = default(Entity);

                if (Entities == null)
                {
                    throw new($"Хранилище пусто.");
                }

                await Task.Factory.StartNew(() =>
                {
                    Entity ent = Entities.Find(a => a.Id.Equals(id));

                    result = ent ?? throw new($"Транзакция с Id {id} не существует.");
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