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

        public IActionResult ShowGrid()
        {
            return View();
        }

        public async Task<JsonResult> LoadData()
        {
            /*
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();

                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;

                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                // getting all Customer data  
                var customerData = _uow.GetGenericRepository<Product>().GetAll().Select(
                    x => new
                    {
                        x.Name,
                        x.ModelNumber,
                        x.Category,
                        x.DescriptionMain,
                        x.DescriptionExtra,
                        x.Price,
                        x.DiscountedPrice,
                        x.Quantity,
                        x.WarrantyStatus
                    });
                //Sorting  
                /*
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                */
            //Search  
            /*
            if (!string.IsNullOrEmpty(searchValue))
            {
                customerData = customerData.Where(m => m.Name == searchValue);
            }

            //total number of rows counts   
            var enumerable = customerData.ToList();
            recordsTotal = enumerable.Count();
            //Paging   
            var data = enumerable.Skip(skip).Take(pageSize).ToList();
            //Returning Json Data  
            return new JsonResult(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

        }
        catch (Exception)
        {
            throw;
        }
        */

            try
            {
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                // Skiping number of Rows count  
                var start = Request.Query["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Query["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Query["columns[" + Request.Query["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Query["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // get entity data from context
                var data = (from tempData in _uow.GetGenericRepository<Product>().GetAll()
                    select tempData);


                //total number of rows count   
                recordsTotal = data.Count();

                //paging   
                var response = data.Skip(skip).Take(pageSize).ToList().Select(
                    x => new
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ModelNumber = x.ModelNumber,
                        Category = x.Category,
                        Price = x.Price,
                        DiscountedPrice = x.DiscountedPrice,
                        Quantity = x.Quantity,
                    }).ToList();


                //Returning Json Data  
                return new JsonResult(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });

            }
            catch (Exception)
            {
                throw;
            }


        }


    }
}