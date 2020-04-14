using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Web.Models.DTOs
{
    public class CartItemsDto
    {
        public IEnumerable<CartITem> Items { get; set; }
    }


    public class CartITem
    {
        public int Id { get; set; }
        public string ItemImageUrl { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public int ItemQuantity { get; set; }
        public double ItemTotalPrice { get; set; }
        
    }

}
