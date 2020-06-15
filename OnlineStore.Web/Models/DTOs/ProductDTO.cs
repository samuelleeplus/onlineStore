using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models.DTOs
{

    public class ProductBase
    {
        [Key] public int ProductId { get; set; }
        [Required] public string Name { get; set; }
        [Required] public double DiscountedPrice { get; set; }

    }

    public class ProductDto : ProductBase
    {
        [Required] public Header Header { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string DescriptionExtra { get; set; }
        [Required] public IEnumerable<string> ImageUrls { get; set; }
        [Required] public double OriginalPrice { get; set; }
        [Required] public string Availability { get; set; } = "In Stock";
        [Required] public IEnumerable<RelatedProduct> RelatedProducts { get; set; }

    }

    public class Header
    {
        [Required] public string BackgroundImageUrl { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string Text { get; set; }
    }


    public class RelatedProduct
    {
        [Required] public string Url { get; set; }
        [Required] public string ImageUrl { get; set; }
        [Required] public string Name { get; set; }
        [Required] public double Price { get; set; }
        [Required] public string StatusClass { get; set; }
    }

    public class InvoiceItemDto : ProductBase
    {
        public int Quantity { get; set; }
        public bool IsDelivered { get; set; }
    }

    public class ProductEditCreateDto : ProductBase
    {
        [Required]
        public int ModelNumber { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string WarrantyStatus { get; set; }
        [Required]
        public string DescriptionExtra { get; set; }
        [Required]
        public string DescriptionMain { get; set; }
        [Required]
        public string DistributorInfo { get; set; }
        [Required]
        public string Category { get; set; }

        public string[] ImageUris { get; set; } = new string[4]{"", "", "", ""};

    }
}
