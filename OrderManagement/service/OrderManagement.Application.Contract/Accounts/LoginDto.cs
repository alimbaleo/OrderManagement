﻿using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Application.Contract.Accounts
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
