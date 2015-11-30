using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCData.Infrastructure;

namespace CCData
{
    public partial class RunnerRaceRecordSegment: IValidatableEntity, ICreatableEntity, IModifiableEntity
    {
        #region SaveValidate
        public IEnumerable<DbValidationError> SaveValidate()
        {
            var errors = new List<DbValidationError>();

            if (StartTime.HasValue && EndTime.HasValue && EndTime < StartTime)
                errors.Add(new DbValidationError("End Time cannot be earlier than Start Time", "EndTime"));

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
