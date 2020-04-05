using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Web.Models.DTOs
{
    public class ProductDTO
    {
        public Header Header { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DescriptionExtra { get; set; }
        public IEnumerable<string> ImageUrls { get; set; }
        public double OriginalPrice { get; set; }
        public double DiscountedPrice { get; set; }
        public string Availability { get; set; } = "In Stock";
        public IEnumerable<RelatedProduct> RelatedProducts { get; set; }

    }

    public class Header
    {
        public string BackgroundImageUrl { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }


    public class RelatedProduct
    {
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string StatusClass { get; set; }
    }



}
