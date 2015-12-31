using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foolproof;
using CCData;

namespace CCMvc.ViewModels
{
    public class RunnerViewModel
    {
        // Make sure to clear this value from the ModelState after creation
        public long? RunnerRaceRecordId { get; set; }
        public long UserId { get; set; }

        public bool RaceIsComplete { get; set; }

        public string Name { get; set; }
        public bool UserIsEnrolled { get; set; }

        [RequiredIfTrue("UserIsEnrolled", ErrorMessage = "Please select Varsity Level")]
        public int? VarsityLevelId { get; set; }

        [RequiredIfTrue("UserIsEnrolled", ErrorMessage = "Please select Classification")]
        public int? RunnerClassificationId { get; set; }

        public SelectList VarsityLevels { get; set; }
        public SelectList RunnerClassifications { get; set; }
    }
}