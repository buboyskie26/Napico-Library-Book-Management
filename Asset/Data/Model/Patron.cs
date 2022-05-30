using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.Data.Model
{
    public class Patron : IdentityUser
    {

        [Display(Name = "First Name")]
        [StringLength(30, ErrorMessage = "Limit first name to 30 characters.")]
        public string FirstName { get; set; }
        public string ImageUrl { get; set; }
        public int OverDueFee { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(30, ErrorMessage = "Limit last name to 30 characters.")]
        public string LastName { get; set; }
        public DateTime DateLogin { get; set; }
        public string Role { get; set; }

        public DateTime Created { get; set; }
        public DateTime CheckoutSpan { get; set; }
        public DateTime Penalty { get; set; }
        public string Address { get; set; }
        public bool? isReturn { get; set; }
        public bool IsAssetCheckout { get; set; }

        [Display(Name = "Library Card")]
        public LibraryCard LibraryCard { get; set; }



    }
}
