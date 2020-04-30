using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Web.Models.DTOs
{
    public class CreditCardDto
    {
        [Required]
        [Display(Name = "Card number", Prompt = "Enter your credit card number")]
        [RegularExpression("^(\\d{4})(-(\\d){4}){2}(-\\d{4})$", ErrorMessage = "Please enter valid credit card")]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "Full name", Prompt = "Enter your full name")]
        [RegularExpression("[a-zA-Z]+([ ])[a-zA-Z]+", ErrorMessage = "Please enter valid name! We are not" +
                                                                     " interested on your nickname")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Expiry Date", Prompt = "MM/YY")]
        [RegularExpression("[0-9]{2}/[0-9]{2}", ErrorMessage = "Please enter a valid expiry date")]
        public string ExpiryDate { get; set; }

        [Required]
        [Display(Name = "CVC", Prompt = "CVC")]
        [RegularExpression("[0-9]{3}", ErrorMessage = "Please enter a valid CVC")]
        public string Cvc { get; set; }
    }
}
