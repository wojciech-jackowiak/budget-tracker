using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public void MarkAsModified()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public bool IsNew => Id == 0;
    }
}
