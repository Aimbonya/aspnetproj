﻿using System.ComponentModel.DataAnnotations;

namespace _253501_mammadov.Models
{
    public class RegisterUserViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        public IFormFile? Avatar { get; set; }
    }
}
