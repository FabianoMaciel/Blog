﻿using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "The username is Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The Password is Required")]
        [MinLength(5)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
