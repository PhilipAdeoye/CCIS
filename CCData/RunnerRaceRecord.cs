using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCData.Infrastructure;
using System.Data.SqlClient;

namespace CCData
{
	public partial class RunnerRaceRecord: IValidatableEntity, IModifiableEntity, ICreatableEntity
	{
		#region SaveValidate
		public IEnumerable<DbValidationError> SaveValidate()
		{
			var errors = new List<DbValidationError>();

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

		#region RunnerData
		public class RunnerData
		{
			// Make sure to clear this value from the ModelState after creation
			public long? RunnerRaceRecordId { get; set; }
			public long RaceId { get; set; }
			public long OrganizationId { get; set; }
			public long UserId { get; set; }

			public string Name { get; set; }
			public bool UserIsEnrolled { get; set; }
			public bool RaceIsComplete { get; set; }

			public int? VarsityLevelId { get; set; }
			public int? RunnerClassificationId { get; set; }
		} 
		#endregion

		#region GetRunnerEnrolmentData
		public static List<RunnerData> GetRunnerEnrolmentData(long raceId, long organizationId)
		{
			using (var db = new CCEntities())
			{
				return db.ExecuteStoreQuery<RunnerData>(@"
							SELECT 
								u.UserId,
								rr.RunnerRaceRecordId,
								u.Firstname + ' ' + u.Lastname AS Name, 
								CAST(IIF(rr.RunnerRaceRecordId IS NOT NULL, 1, 0) AS BIT) AS UserIsEnrolled,
								CAST(IIF(r.CompletedOn IS NOT NULL, 1, 0) AS BIT) AS RaceIsComplete,
								ISNULL(rr.VarsityLevelId, u.DefaultVarsityLevelId) AS VarsityLevelId,
								ISNULL(rr.RunnerClassificationId, u.DefaultRunnerClassificationId) AS RunnerClassificationId

							FROM Race r 
								INNER JOIN Organization o ON r.OrganizationId = o.OrganizationId
								INNER JOIN [User] u ON u.OrganizationId = o.OrganizationId
								LEFT OUTER JOIN RunnerRaceRecord rr ON u.UserId = rr.UserId
							WHERE r.RaceId = @raceId AND r.OrganizationId = @organizationId
								AND (u.Gender = r.GenderRestriction OR r.GenderRestriction = '" + Genders.Unspecified + "')",
						new SqlParameter[] {
							new SqlParameter("raceId", raceId),
							new SqlParameter("organizationId", organizationId)
						}).ToList();
			}
		} 
		#endregion

		#region UnEnrollRunners
		public static void UnEnrollRunners(IEnumerable<long> unenrolledRunnerIds)
		{
			if (unenrolledRunnerIds.Count() > 0)
			{
				using (var db = new CCEntities())
				{
					var ids = string.Join(",", unenrolledRunnerIds);
					db.ExecuteStoreCommand(@"
						DELETE FROM RunnerRaceRecordSegment 
						WHERE RunnerRaceRecordId IN(" + ids + @");
						
						DELETE FROM RunnerRaceRecord
						WHERE RunnerRaceRecordId IN(" + ids + @");");
				}
			}
		} 
		#endregion
	}
}
