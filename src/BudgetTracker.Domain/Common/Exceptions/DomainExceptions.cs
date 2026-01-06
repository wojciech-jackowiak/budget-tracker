using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetTracker.Domain.Common.Exceptions
{

    /// <summary>
    /// Wyjątek reprezentujący naruszenie reguł biznesowych w warstwie Domain.
    /// Używany gdy operacja nie spełnia wymagań domenowych (validation, business rules).
    /// </summary>
    public class DomainException : Exception
    {
        public string? ErrorCode { get; }

        public DomainException(string message)
            : base(message)
        {
        }

        public DomainException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public DomainException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public DomainException(string message, string errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
