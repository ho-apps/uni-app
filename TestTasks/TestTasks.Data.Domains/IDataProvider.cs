using System;
using System.Threading.Tasks;

namespace TestTasks.Data.Domains
{
    /// <summary>
    /// Интерфейс провайдера хранения данных
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Создание записи для хранения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task CreateAsync<T>(T entity);
        /// <summary>
        /// Поиск по id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindByIdAsync<T>(string id);
    }
}
