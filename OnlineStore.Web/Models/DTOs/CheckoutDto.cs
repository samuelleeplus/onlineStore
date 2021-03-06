using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models.DTOs
{
    public class CheckoutDto
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }

        public AddressDto AddressInfo { get; set; }

        [Required, Phone] public string Phone { get; set; }

        [Required, EmailAddress] public string Email { get; set; }
        
        [Display(Name = "I agree to the terms and conditions")]
        [Range(typeof(bool), "true", "true",
            ErrorMessage = "You must agree to the terms and conditions.")]
        public bool IsAgreeTermsAndConditions { get; set; }

        public double Shipping { get; set; }
        public IEnumerable<CheckoutItem> CheckoutItems { get; set; }
    }

    public class CheckoutItem
    {
        public string ProductName { get; set; }
        public double Price { get; set; }
    }
}

