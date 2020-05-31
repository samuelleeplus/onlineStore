using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Web.Models.DTOs
{
    public class ReviewDto
    {
        public int Rating { get; set; }
        public string Review { get; set; }
    }
}
