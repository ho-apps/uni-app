using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TestTasks.Data.Domains.Models;

namespace TestTasks.Data.Domains;

public class EntityDataProvider
{
    private readonly ILogger<EntityDataProvider> _logger;

    public EntityDataProvider(ILogger<EntityDataProvider> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        Entities = new List<Entity>();
    }

    private List<Entity> Entities { get; }

    /// <summary>
    ///     Создание записи в хранилище данных
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
                var obj = JsonConvert.DeserializeObject<Entity>(data);

                // если запись с таким Id уже существует, выбрасываем исключение
                if (Entities.Exists(a => a.Id == obj.Id)) throw new Exception("Запись с таким Id уже существует.");

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
    ///     Поиск по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Entity> FindByIdAsync(Guid id)
    {
        try
        {
            var result = default(Entity);

            if (Entities == null) throw new Exception("Хранилище пусто.");

            await Task.Factory.StartNew(() =>
            {
                var ent = Entities.Find(a => a.Id.Equals(id));

                result = ent ?? throw new Exception($"Транзакция с Id {id} не существует.");
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