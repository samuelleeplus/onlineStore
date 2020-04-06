using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Web.Models.DTOs
{
    public class CategoryDTO
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string BackgroundImageUrl { get; set; }
        public IEnumerable<RelatedProduct> Products { get; set; }
    }
}
