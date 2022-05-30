﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.ViewModel
{
    public class RegisterVM
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Address { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Contact Number")]
        public string contactNumber { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Display(Name = "Birth Day")]
        public DateTime Birthday { get; set; } = DateTime.UtcNow;


        [Display(Name = "Joined Date")]
        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Full Name")]
        public string Fullname
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        [Display(Name = "Photo")]
        public IFormFile ImageUrl { get; set; }
    }
}
