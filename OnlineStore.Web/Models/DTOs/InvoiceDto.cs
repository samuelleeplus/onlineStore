using System.Collections.Generic;
using OnlineStore.Data.Models.Entities;

namespace OnlineStore.Web.Models.DTOs
{
    public class InvoiceDto
    {
        public string CustomerName { get; set; }
        public Address CustomerAddress { get; set; }
        public int OrderId { get; set; }
    //    public IEnumerable<InvoiceItemDto> InvoiceItems { get; set; }

        public IEnumerable<CartItem> Items { get; set; }

        public double totalPrice { get; set; }

    }

}
