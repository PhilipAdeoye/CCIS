using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CCMvc.ViewModels
{
    public class ErrorViewModel
    {
        [Required(ErrorMessage = "A message is required")]
        public string Message { get; set; }

        [Display(Name = "Your Email")]
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Not a valid email address")]
        public string Email { get; set; }
    }
}