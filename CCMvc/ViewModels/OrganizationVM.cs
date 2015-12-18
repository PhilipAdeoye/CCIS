using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCMvc.ViewModels
{
    public class OrganizationVM
    {
        public long OrganizationId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Name = "Mascot Name")]
        public string MascotName { get; set; }

        public string Tagline { get; set; }

        public string MascotImageFileLocation { get; set; }
    }
}