using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OnlineStore.Data.Context;
using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Repositories;


namespace OnlineStore.Web.Controllers
{
    public class SalesManagerController : Controller
    {
        private ApplicationDbContext _context { get; set; }
        private UnitOfWork _uow { get; set; }

        public SalesManagerController(ApplicationDbContext context)
        {
            _context = context;
            _uow = new UnitOfWork(_context);
        }

        public ActionResult Index()
        {
            return RedirectToAction("ListProducts");
        }
        public IActionResult ListProducts()
        {
            var repo = _uow.GetGenericRepository<Product>();
            var products = repo.GetAll();

            return View(products);
        }


        // POST: SalesManager/updatePrice/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult updatePrice(Product product,int discount)
        {
            var price = product.Price;
            product.DiscountedPrice = price - ((price * discount) / 100);
            return RedirectToAction("Index");
        }
    }
}