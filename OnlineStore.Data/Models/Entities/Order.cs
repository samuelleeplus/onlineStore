﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineStore.Data.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<OrderedProduct> OrderedProducts { get; set; }
        public int TotalPrice { get; set; }
        public string Address { get; set; }
        public bool IsDelivered { get; set; }
    }


    public class OrderedProduct
    {
        [Key]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }


}
