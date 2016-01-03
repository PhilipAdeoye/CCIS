using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCMvc.ViewModels
{
    public class ManageSplitsViewModel
    {
        public long RunnerRaceRecordId { get; set; }
        public List<SplitViewModel> Splits { get; set; }
    }
}