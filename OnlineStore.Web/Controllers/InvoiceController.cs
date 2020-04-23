using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data.Context;
using OnlineStore.Data.Repositories;
using OnlineStore.Web.Helpers;
using OnlineStore.Web.Models.DTOs;
using Rotativa.AspNetCore;
using Wkhtmltopdf.NetCore;

namespace OnlineStore.Web.Controllers
{
    public class InvoiceController : Controller
    {
        private ApplicationDbContext _context { get; }
        private UnitOfWork _uow { get; }

        public InvoiceController(ApplicationDbContext context)
        {
            _context = context;
            _uow = new UnitOfWork(_context);
        }
        /*
        public async Task<IActionResult> Index()
        {
            
        }
        */

        public IActionResult InvoiceHtml()
        {
            return View("Invoice", new InvoiceDto());
        }

        public IActionResult InvoicePdf(int id)
        {
            var prodCtor = new DtoConstructor(_uow);

            var model = prodCtor.InvoiceDtoByCustomerID(id);

            return new ViewAsPdf("Invoice", model);
        }

    }
}
