using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Blog.Core.Models
{
    public class UserInsertModel
    {
        [Required(ErrorMessage = "The Email is Required")]
        [EmailAddress(ErrorMessage = "The format is not valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password is Required")]
        [StringLength(100, ErrorMessage = "Campo {0} precisa ter entre {1} e {2} caracteres.", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "The passwords are not matching")]
        public string ConfirmPassword { get; set; }

        public bool IsAdmin { get; set; }
    }
}
