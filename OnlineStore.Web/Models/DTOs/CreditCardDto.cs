using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models.DTOs
{
    public class CreditCardDto
    {
        [Required]
        [Display(Name = "Card number: XXXX-XXXX-XXXX-XXXX", Prompt = "XXXX-XXXX-XXXX-XXXX")]
        [RegularExpression("^(\\d{4})(-(\\d){4}){2}(-\\d{4})$", ErrorMessage = "Please enter valid credit card")]
        public string CardNumber { get; set; }

        [Required]
        [Display(Name = "Full name", Prompt = "Enter your full name")]
        [RegularExpression("[a-zA-Z]+([ ])[a-zA-Z]+", ErrorMessage = "Please enter valid name! We are not" +
                                                                     " interested on your nickname")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Expiry Date: MM/YY", Prompt = "MM/YY")]
        [RegularExpression("[0-9]{2}/[0-9]{2}", ErrorMessage = "Please enter a valid expiry date")]
        public string ExpiryDate { get; set; }

        [Required]
        [Display(Name = "CVC: XXX", Prompt = "CVC")]
        [RegularExpression("[0-9]{3}", ErrorMessage = "Please enter a valid CVC")]
        public string Cvc { get; set; }

        public double Shipping { get; set; }
        public IEnumerable<CheckoutItem> CheckoutItems { get; set; }
    }
}
