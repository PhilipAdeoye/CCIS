using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CCData;
using System.Web.Mvc;

namespace CCMvc.ViewModels
{
    public class RaceCreateViewModel
    {        
        public long OrganizationId { get; set; }

        [Display(Name = "Name")]
        public string Description { get; set; }

        public string Remarks { get; set; }

        [Display(Name="Starts At")]
        [Required(ErrorMessage="Please provide a start time")]
        public DateTime? StartsAtUTC { get; set; }

        [Display(Name = "Restricted To")]
        [Required(ErrorMessage = "A Gender Restriction is required. Use '"
            + Genders.Unspecified + "' for non-specific races")]
        public string GenderRestriction { get; set; }

        public SelectList GenderList
        {
            get
            {
                return new SelectList(new List<SelectListItem>()
                {
                    new SelectListItem() { Value = Genders.Female, Text = Genders.Female},
                    new SelectListItem() { Value = Genders.Male, Text = Genders.Male},
                    new SelectListItem() { Value = Genders.Unspecified, Text = Genders.Unspecified}
                }, "Value", "Text", GenderRestriction);
            }
        }
    }
}