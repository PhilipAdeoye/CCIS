using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCData
{
    public class DbValidationException : Exception
    {
        public DbValidationException()
        {
        }

        public DbValidationException(string message)
            : base(message)
        {
        }

        public DbValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public DbValidationException(IEnumerable<DbValidationError> errors)
        {            
            DbValidationErrors = errors;
        }

        public IEnumerable<DbValidationError> DbValidationErrors { get; set; }
    }
}
