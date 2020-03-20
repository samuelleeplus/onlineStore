using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Models.Entities
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ModelNumber { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string WarrantyStatus { get; set; }
        public string DistributorInfo { get; set; }
    }
}
