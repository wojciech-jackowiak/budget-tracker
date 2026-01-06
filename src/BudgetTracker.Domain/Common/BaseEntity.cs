using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; } 
        public DateTime CreatedAt { get; private set; } 
        public DateTime? UpdatedAt { get; private set; }

        protected BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public void MarkAsModified()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsNew => Id == 0;
    }
}
