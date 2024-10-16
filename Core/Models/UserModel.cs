using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Blog.Core.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "The Email is Required")]
        [EmailAddress]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password is Required")]
        [MinLength(5)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public string Occupation { get; set; }

        [Display(Name = "Administrator")]
        public bool IsAdmin { get; set; }

        [BindProperty, DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [BindProperty, DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}
