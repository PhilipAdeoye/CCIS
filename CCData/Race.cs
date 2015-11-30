using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCData.Infrastructure;

namespace CCData
{
    public partial class Race: IValidatableEntity, ICreatableEntity, IModifiableEntity
    {
        #region SaveValidate
        public IEnumerable<DbValidationError> SaveValidate()
        {
            var errors = new List<DbValidationError>();

            if (string.IsNullOrWhiteSpace(Description))
                errors.Add(new DbValidationError("Description is required", "Description"));

            if (StartedOn.HasValue && CompletedOn.HasValue && CompletedOn < StartedOn)
                errors.Add(new DbValidationError("Completed On cannot be earlier than Started On", "CompletedOn"));                        

            return errors;
        }
        #endregion

        #region DeleteValidate
        public IEnumerable<DbValidationError> DeleteValidate()
        {
            return new List<DbValidationError> { };
        }
        #endregion
    }
}
