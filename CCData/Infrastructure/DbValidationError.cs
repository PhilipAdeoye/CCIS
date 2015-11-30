using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCData.Infrastructure
{
    public class DbValidationError
    {
        public DbValidationError() { }
        public DbValidationError(string message, string propertyName)
        {
            Message = message;
            PropertyName = propertyName;
        }

        public string PropertyName { get; set; }
        public string Message { get; set; }
    }
}
