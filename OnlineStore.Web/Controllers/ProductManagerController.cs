using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data.Context;
using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Repositories;

namespace OnlineStore.Web.Controllers
{
    public class ProductManagerController : Controller
    {

        private ApplicationDbContext _context { get; set; }
        private UnitOfWork _uow { get; set; }

        public ProductManagerController(ApplicationDbContext context)
        {
            _context = context;
            _uow = new UnitOfWork(_context);
        }


        // GET: ProductManager
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



        
        // GET: ProductManager/Details/5
        public ActionResult Details(int id)
        {
            var repo = _uow.GetGenericRepository<Product>();
            var product = repo.GetById(id);

            return View(product);
        }

        // GET: ProductManager/Create
        public ActionResult Create()
        {
            return View();
        }
        /*
        // POST: ProductManager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        */
        // GET: ProductManager/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        /*
        // POST: ProductManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        */
        // GET: ProductManager/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        
        // POST: ProductManager/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
    }
}