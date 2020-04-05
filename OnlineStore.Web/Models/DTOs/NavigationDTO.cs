using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Web.Models.DTOs
{
    public class NavigationDTO
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}
