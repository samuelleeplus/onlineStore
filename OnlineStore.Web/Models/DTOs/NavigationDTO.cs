using System.Collections.Generic;

namespace OnlineStore.Web.Models.DTOs
{
    public class NavigationDto
    {
        public IEnumerable<Category> Categories { get; set; }
        public int NumberOfItemsInCart { get; set; }

        public string Username { get; set; }
    }
}
