using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineStore.Data.Models.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Address { get; set; }
    }
}
