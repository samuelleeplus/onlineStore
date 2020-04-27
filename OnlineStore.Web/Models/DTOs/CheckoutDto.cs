using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Web.Models.DTOs
{
    public class CheckoutDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string AddressDetail { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required] public bool AgreeTermsAndConditions { get; set; }

    }
}
