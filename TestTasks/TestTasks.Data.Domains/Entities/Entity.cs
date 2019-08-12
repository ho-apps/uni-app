using System;
using System.Collections.Generic;
using System.Text;

namespace TestTasks.Data.Domains.Entities
{
    public class Entity
    {
        public Guid Id { get; set; }
        public DateTime OperationDate { get; set; }
        public decimal Amount { get; set; }
        public Entity()
        {
            Id = Guid.NewGuid();
            OperationDate = DateTime.Now;
        }
    }
}
