using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Application.Tests.Common
{
    public static class TestHelpers
    {
        /// <summary>
        /// Ustawia private property przez reflection (tylko dla testów!)
        /// </summary>
        public static void SetPrivateProperty<T>(object obj, string propertyName, T value)
        {
            var property = obj.GetType().GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{obj.GetType().Name}'");
            }
            property.SetValue(obj, value);
        }

        /// <summary>
        /// Ustawia Id encji (tylko dla testów!)
        /// </summary>
        public static void SetId<TEntity>(TEntity entity, int id) where TEntity : class
        {
            SetPrivateProperty(entity, "Id", id);
        }
    }
}
