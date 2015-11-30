using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCData.Infrastructure;

namespace CCData
{
    public partial class RunnerRaceRecord: IValidatableEntity, IModifiableEntity, ICreatableEntity
    {
        #region SaveValidate
        public IEnumerable<DbValidationError> SaveValidate()
        {
            var errors = new List<DbValidationError>();

            if (StartTime.HasValue && EndTime.HasValue && EndTime < StartTime)
                errors.Add(new DbValidationError("End Time cannot be earlier than Start Time", "EndTime"));

            if (errors.Count == 0)
            {
                using (var db = new CCEntities())
                {
                    if (!db.Races.Any(r => r.RaceId == RaceId))
                        errors.Add(new DbValidationError("Invalid Race", "RaceId"));

                    if(!db.RunnerClassifications.Any(rc=> rc.RunnerClassificationId == RunnerClassificationId))
                        errors.Add(new DbValidationError("Invalid Runner Classification", "RunnerClassificationId"));

                    if (!db.VarsityLevels.Any(v => v.VarsityLevelId == VarsityLevelId))
                        errors.Add(new DbValidationError("Invalid Varsity Level", "VarsityLevelId"));
                }
            }

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
