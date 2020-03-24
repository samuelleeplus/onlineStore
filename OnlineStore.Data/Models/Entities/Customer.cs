using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Models.Entities
{
    class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public string EmailAddress { get; set; }
        public string HomeAddress { get; set; }
        public string Password { get; set; }

    }
}
