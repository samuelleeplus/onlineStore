using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data.Context;
using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Repositories;
using OnlineStore.Web.Models.DTOs;

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
            return RedirectToAction("ShowGrid");
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
            var product = _uow.GetGenericRepository<Product>().GetById(id);

            var distributor = _uow.GetGenericRepository<Distributor>()
                .FirstOrDefault(x => x.Id == product.DistributorId);

            var images = _uow.GetGenericRepository<ImageUri>().Find(x => x.ProductId == id).Take(4).ToList();
            var imageUris = new string[]{"", "", "", ""};

            for (int i = 0; i < images.Count; i++)
            {
                imageUris[i] = images[i].Uri;
            }

            // TODO: delete imageUris fetched to prevent ...

            if (product != null)
            {
                var model = new ProductEditCreateDto()
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    DiscountedPrice = product.DiscountedPrice,
                    ModelNumber = product.ModelNumber,
                    Quantity = product.Quantity,
                    WarrantyStatus = product.WarrantyStatus,
                    DescriptionMain = product.DescriptionMain,
                    DescriptionExtra = product.DescriptionExtra,
                    DistributorInfo = distributor?.Info ?? "",
                    ImageUris = imageUris, /*new string[]
                    {
                        "https://pbs.twimg.com/media/ENL2osHX0AAE1LW?format=jpg&name=900x900",
                        "https://pbs.twimg.com/media/ENL2osHX0AAE1LW?format=jpg&name=900x900",
                        "https://pbs.twimg.com/media/ENL2osHX0AAE1LW?format=jpg&name=900x900",
                        "https://pbs.twimg.com/media/ENL2osHX0AAE1LW?format=jpg&name=900x900"
                    }, */
                    Category = product.Category
                };
                return View(model);
            }
            return View(new ProductEditCreateDto());
        }
        
        // POST: ProductManager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductEditCreateDto _product)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var imageRepository = _uow.GetGenericRepository<ImageUri>();
                    imageRepository.RemoveIf(x => x.ProductId == id);

                    foreach (var uri in _product.ImageUris)
                    {
                        if (!String.IsNullOrEmpty(uri))
                        {
                            imageRepository.Add(new ImageUri()
                            {
                                ProductId = id,
                                Uri = uri
                            });
                        }
                    }

                    var distRepo = _uow.GetGenericRepository<Distributor>();
                    var distributor = new Distributor();
                    if (!String.IsNullOrEmpty(_product.DistributorInfo))
                    {
                        distributor.Info = _product.DistributorInfo;
                    }
                    distRepo.Add(distributor);
                    _uow.Commit();

                    var productRepository = _uow.GetGenericRepository<Product>();
                    var product = productRepository.GetById(id);
                    
                    product.Name = _product.Name;
                    product.Category = _product.Category;
                    product.Price = _product.Price;
                    product.Quantity = _product.Quantity;
                    product.DiscountedPrice = _product.DiscountedPrice;
                    product.WarrantyStatus = _product.WarrantyStatus;
                    product.ModelNumber = _product.ModelNumber;
                    product.DescriptionMain = product.DescriptionMain;
                    product.DescriptionExtra = _product.DescriptionExtra;
                    product.DistributorId = distributor.Id;

                    productRepository.Update(product);
                    _uow.Commit();
                    return RedirectToAction("Edit", new{ id = id });
                }

                return View(); // error message should be returned.
            }
            catch
            {
                return View();
            }
        }

        // POST: ProductManager/Delete/5
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                var repo = _uow.GetGenericRepository<Product>();
                var product = repo.GetById(id);
                repo.Remove(product);
                _uow.Commit();
                return Json(new { success = true, responseText = "Successfully deleted!" });
            }
            catch (Exception e)
            {
                return NotFound();
            }

        }


        public IActionResult ShowGrid()
        {
            return View();
        }

        public async Task<JsonResult> LoadData()
        {
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

                // Sorting
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                     //.OrderBy(sortColumn + " " + sortColumnDirection).ToList();
                     var propertyInfo = typeof(Product).GetProperty(sortColumn);
                     if (sortColumnDirection == "asc")
                     {
                         data = data.OrderBy(x => propertyInfo.GetValue(x, null));
                     }
                     else if (sortColumnDirection == "desc")
                     {
                         data = data.OrderByDescending(x => propertyInfo.GetValue(x, null));
                     }
                }
                // searching
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.Name.ToUpper().Contains(searchValue.ToUpper()));
                }
                
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