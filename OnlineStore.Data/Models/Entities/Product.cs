using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Data.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModelNumber { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string WarrantyStatus { get; set; }
        public string DescriptionExtra { get; set; }
        public string DescriptionMain { get; set; }
        public double DiscountedPrice { get; set; }
        public int DistributorId { get; set; }
        public string Category { get; set; }
    }

    public class ImageUri
    {
        public int Id { get; set; }
        public string Uri { get; set; }
        public int ProductId { get; set; }
    }

    public class Distributor
    {
        public int Id { get; set; }
        public string Info { get; set; }
    }
}
