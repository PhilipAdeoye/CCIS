using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCMvc.ViewModels
{
    public class UserViewModel
    {
        public long UserId { get; set; }
        public long OrganizationId { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }

        [Display(Name = "Eligible For Races")]
        public bool EligibleForRaces { get; set; }

        public bool CanBeDeleted { get; set; }
    }
}