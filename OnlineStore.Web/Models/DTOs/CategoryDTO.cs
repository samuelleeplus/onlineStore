using System.Collections.Generic;

namespace OnlineStore.Web.Models.DTOs
{
    public class CategoryDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string BackgroundImageUrl { get; set; }
        public IEnumerable<SimpleProduct> Products { get; set; }
        public IEnumerable<int> Pages { get; set; }
    }
}
