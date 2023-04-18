using System;
using System.Runtime.Serialization;

namespace TestTasks.Data.Domains.Models;

/// <summary>
///     Сущность Транзакции
/// </summary>
public class Transaction
{
    /// <summary>
    ///     Идентификатор
    /// </summary>
    [DataMember]
    public int Id { get; set; }

    /// <summary>
    ///     Дата транзакции
    /// </summary>
    [DataMember]
    public DateTime TransactionDate { get; set; }

    /// <summary>
    ///     Сумма
    /// </summary>
    [DataMember]
    public decimal Amount { get; set; }
}