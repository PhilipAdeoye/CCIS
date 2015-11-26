using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Validation;

namespace CCData
{
    public abstract class BaseEntity
    {
        public virtual List<DbValidationError> SaveValidate(CCEntities db)
        {
            throw new NotImplementedException("Method Not Implemented: SaveValidate");
        }

        public virtual List<DbValidationError> DeleteValidate(CCEntities db)
        {
            throw new NotImplementedException("Method Not Implemented: DeleteValidate");
        }
    }
}
