﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteFileStorage.Models
{
    public class LogOnViewModel
    {
        [Required(ErrorMessage = "Please, enter your login!")]
        [Display(Name = "Login")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Please, enter your password!")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}