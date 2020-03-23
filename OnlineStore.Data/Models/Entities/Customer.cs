using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Models.Entities
{
    class Customer
    {
        public int customer_id { get; set; }
        public string name { get; set; }
        public int product_Id { get; set; }
        public string email_address { get; set; }
        public string home_address { get; set; }
        public string password { get; set; }

    }
}
