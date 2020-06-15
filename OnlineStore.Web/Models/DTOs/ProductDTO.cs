﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Web.Models.DTOs
{
    public class ProductDto
    {
        [Required] public int ProductId { get; set; }
        [Required] public Header Header { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string DescriptionExtra { get; set; }
        [Required] public IEnumerable<string> ImageUrls { get; set; }
        [Required] public double OriginalPrice { get; set; }
        [Required] public double DiscountedPrice { get; set; }
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
}
