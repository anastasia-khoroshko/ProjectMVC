using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteFileStorage.Models
{
    public class UserViewModel
    {
         [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please, enter login!")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Value must be between 5 to 10 characters")]
        [Display(Name = "User name")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Please, enter e-mail!")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Incorrect e-mail")]
        public string Email { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Remote("ValidateDate","Admin")]
        public DateTime CreatedDate { get; set; }
        [Display(Name="Role")]
        public string Role { get; set; }
    }
}