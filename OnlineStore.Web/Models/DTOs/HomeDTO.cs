using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Web.Models.DTOs
{
    public class HomeDto
    {
        [Required] public FrontImage FrontImage { get; set; } // next to the discount image block shows off a front image
        [Required] public DiscountImage DiscountImage { get; set; }
        [Required] public IEnumerable<SimpleProduct> Products { get; set; }
        [Required] public IEnumerable<FrontImage> FrontImages { get; set; } // first block that shows gallery of front images

    }
    public class SimpleProduct
    {
        [Required] public string Name { get; set; }
        [Required] public string ImageUrl { get; set; }
        [Required] public string Url { get; set; }
        [Required] public double OriginalPrice { get; set; }
        [Required] public double DiscountedPrice { get; set; }
        [Required] public string StatusClass { get; set; }

    }
    public class FrontImage
    {
        [Required] public string BackgroundImageUrl { get; set; }
        [Required] public string Title { get; set; }
        [Required] public string Text { get; set; }
    }


    public class DiscountImage
    {
        [Required] public string Url { get; set; } // goes to that category
        [Required] public string ImageUrl { get; set; }
        [Required] public string CategoryName { get; set; }
        [Required] public int DiscountAmount { get; set; } // calculate by using product and discounted price
    }
}
