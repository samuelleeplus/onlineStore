using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Models.Entities
{
    public class CreditCard
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }

        public string FullName { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvc { get; set; }

        public int CustomerId { get; set; }

    }
}
