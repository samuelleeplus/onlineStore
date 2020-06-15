using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Web.Models.DTOs
{
    public class AddressDto
    {
        [Required]
        [Display(Name = "Address")]
        public string AddressDetail { get; set; }
        
        [Required] 
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        
        [Required]
        [Display(Name = "City")]
        public string City { get; set; }
        
        [Required] 
        [Display(Name = "Province")]
        public string Province { get; set; }
    }

    public class AddressDtoChooseOrCreate
    {
        public AddressDto AddressDtoCreate { get; set; }
        public IEnumerable<AddressDto> AddressDtoChoose { get; set; }
    }
}
