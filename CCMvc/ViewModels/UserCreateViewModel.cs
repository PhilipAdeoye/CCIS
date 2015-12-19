using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CCMvc.ViewModels
{
    public class UserCreateViewModel
    {
        public long OrganizationId { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "User Name is required.")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string Firstname { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string Lastname { get; set; }

        [Display(Name = "Middle Name")]        
        public string Middlename { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

        [Display(Name = "Grad. Year")]
        public string GraduationYear { get; set; }

        [Required(ErrorMessage = "User Role is required.")]
        public int? RoleId { get; set; }

        public SelectList RoleList { get; set; }
    }
}