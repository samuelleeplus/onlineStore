using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Models.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string AddressDetail { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public int UserId { get; set; }
    }
}
