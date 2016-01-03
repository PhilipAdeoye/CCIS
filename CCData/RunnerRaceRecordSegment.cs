using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCData.Infrastructure;
using System.Data.SqlClient;

namespace CCData
{
    public partial class RunnerRaceRecordSegment : IValidatableEntity, ICreatableEntity, IModifiableEntity
    {
        #region SaveValidate
        public IEnumerable<DbValidationError> SaveValidate()
        {
            var errors = new List<DbValidationError>();

            if (ElapsedTimeInSeconds < 1)
                errors.Add(new DbValidationError("No one can cover distance in no time", "ElapsedTimeInSeconds"));

            return errors;
        }
        #endregion

        #region DeleteValidate
        public IEnumerable<DbValidationError> DeleteValidate()
        {
            return new List<DbValidationError> { };
        }
        #endregion

        #region GetSplitsForRunnerRaceRecord
        public static List<Split> GetSplitsForRunnerRaceRecord(long runnerRaceRecordId)
        {
            using (var db = new CCEntities())
            {
                var rrrs = db.RunnerRaceRecordSegments.Where(r => r.RunnerRaceRecordId == runnerRaceRecordId)
                    .OrderBy(rrs => rrs.ElapsedTimeInSeconds).ToArray();
                var splits = new List<Split>();

                for (int i = 0; i < rrrs.Length; i++)
                {
                    var split = new Split
                    {
                        RunnerRaceRecordSegmentId = rrrs[i].RunnerRaceRecordSegmentId,
                        ElapsedTimeInSeconds = rrrs[i].ElapsedTimeInSeconds,
                    };
                    if (i == 0)
                        split.IntervalFromPriorSplit = rrrs[i].ElapsedTimeInSeconds;
                    else
                        split.IntervalFromPriorSplit = rrrs[i].ElapsedTimeInSeconds - rrrs[i - 1].ElapsedTimeInSeconds;

                    splits.Add(split);
                }

                return splits;
            }
        } 
        #endregion

        #region Split
        public class Split
        {
            public long RunnerRaceRecordSegmentId { get; set; }
            public int ElapsedTimeInSeconds { get; set; }
            public int IntervalFromPriorSplit { get; set; }
        }
        #endregion

        #region DeleteSegmentsByRunnerRaceRecordId
        public static void DeleteSegmentsByRunnerRaceRecordId(long runnerRaceRecordId)
        {
            using (var db = new CCEntities())
            {
                db.ExecuteStoreCommand(@"
                    DELETE FROM RunnerRaceRecordSegment WHERE RunnerRaceRecordId = @runnerRaceRecordId",
                    new SqlParameter("runnerRaceRecordId", runnerRaceRecordId));
            }
        } 
        #endregion
    }
}
