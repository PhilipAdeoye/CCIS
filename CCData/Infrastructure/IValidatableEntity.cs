﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCData.Infrastructure
{
    interface IValidatableEntity
    {
        IEnumerable<DbValidationError> SaveValidate();
        IEnumerable<DbValidationError> DeleteValidate();
    }    
}
