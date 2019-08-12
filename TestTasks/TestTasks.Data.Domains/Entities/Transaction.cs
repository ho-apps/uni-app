using System;
using System.Collections.Generic;
using System.Text;

namespace TestTasks.Data.Domains.Entities
{
    /// <summary>
    /// Сущность Транзакции
    /// </summary>
   public class Transaction
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Дата транзакции
        /// </summary>
        public DateTime TransactionDate { get; set; }
        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Amount { get; set; }
    }
}
