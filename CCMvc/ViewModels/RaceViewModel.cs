using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCMvc.ViewModels
{
    public class RaceViewModel
    {
        public long RaceId { get; set; }
        public long OrganizationId { get; set; }

        [Display(Name = "Name")]
        public string Description { get; set; }
                
        public int Runners { get; set; }

        public string Remarks { get; set; }

        public DateTime? StartedOnUTC { get; set; }

        public DateTime? CompletedOnUTC { get; set; }

        public bool CanBeDeleted { get; set; }
    }
}