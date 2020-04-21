using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Web.Models.DTOs;
using Rotativa.AspNetCore;
using Wkhtmltopdf.NetCore;

namespace OnlineStore.Web.Controllers
{
    public class InvoiceController : Controller
    {

        /*
        public async Task<IActionResult> Index()
        {
            
        }
        */

        public IActionResult InvoiceHtml()
        {
            return View("Invoice", new InvoiceDto());
        }

        public IActionResult InvoicePdf()
        {
            return new ViewAsPdf("Invoice", new InvoiceDto());
        }

    }
}