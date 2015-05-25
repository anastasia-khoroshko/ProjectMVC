using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteFileStorage.Models
{
    public class RegisterViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please, enter your login!")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Value must be between 5 to 10 characters")]
        [Display(Name = "User name")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Please, enter your e-mail!")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Incorrect e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Value must be between 3 to 15 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter confirm password")]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don`t match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Captcha { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
    }
}