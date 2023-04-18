using System;

namespace TestTasks.Data.Domains.Models;

public class Entity
{
    public Entity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public decimal Amount { get; set; }
}