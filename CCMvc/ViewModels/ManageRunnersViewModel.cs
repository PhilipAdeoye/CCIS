using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCMvc.ViewModels
{
    public class ManageRunnersViewModel
    {
        public long RaceId { get; set; }
        public long OrganizationId { get; set; }

        
        public List<RunnerViewModel> Runners { get; set; }        

    }
}