using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCData.Infrastructure;
using System.Data.SqlClient;

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

            if (!(GenderRestriction == Genders.Male || GenderRestriction == Genders.Female 
                    || GenderRestriction == Genders.Unspecified))
                errors.Add(new DbValidationError("A Gender Restriction is required. Open Races can be '" 
                    + Genders.Unspecified + "'", "GenderRestriction"));

            return errors;
        }
        #endregion

        #region DeleteValidate
        public IEnumerable<DbValidationError> DeleteValidate()
        {
            return new List<DbValidationError> { };
        }
        #endregion

        #region DeleteWithId
        public static void DeleteWithId(long raceId)
        {
            using (var db = new CCEntities())
            {
                var runnerRaceRecordIds = db.RunnerRaceRecords.Where(rr => rr.RaceId == raceId)
                    .Select(rr => rr.RunnerRaceRecordId);

                if (runnerRaceRecordIds.Count() > 0)
                {
                    db.ExecuteStoreCommand(@"
                        DELETE FROM RunnerRaceRecordSegment 
                        WHERE RunnerRaceRecordId IN(" + string.Join(",", runnerRaceRecordIds) + ");");
                }

                db.ExecuteStoreCommand(@"
                    DELETE FROM RunnerRaceRecord WHERE RaceId = @raceId;

                    DELETE FROM Race WHERE RaceId = @raceId;",
                new SqlParameter("raceId", raceId));
            }
        } 
        #endregion
    }
}
