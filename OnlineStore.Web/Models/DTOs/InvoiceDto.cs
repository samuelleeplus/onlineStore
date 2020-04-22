using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Data.Models.Entities;

namespace OnlineStore.Web.Models.DTOs
{
    public class InvoiceDto
    {
        public string CustomerName { get; set; }
        public Address CustomerAddress { get; set; }
        public int OrderId { get; set; }
        public IEnumerable<InvoiceItemDto> InvoiceItems { get; set; }

    }
}
