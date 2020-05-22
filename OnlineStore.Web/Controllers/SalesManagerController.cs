using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OnlineStore.Data.Context;
using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Repositories;
using OnlineStore.Web.Models.DTOs;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace OnlineStore.Web.Controllers
{
    public class SalesManagerController : Controller
    {
        private ApplicationDbContext _context { get; set; }
        private UnitOfWork _uow { get; set; }

        //
        private readonly IConfiguration _configuration;

        public SalesManagerController(ApplicationDbContext context,IConfiguration configuration)
        {
            _context = context;
            _uow = new UnitOfWork(_context);
            _configuration = configuration; 
        }

        public ActionResult Index()
        {
            //return RedirectToAction("ShowGrid");
            return View();
        }
        public IActionResult ListProducts()
        {
            var repo = _uow.GetGenericRepository<Product>();
            var products = repo.GetAll();

            return View(products);
        }


        public ActionResult Edit(int id)
        {
            var product = _uow.GetGenericRepository<Product>().GetById(id);

            var distributor = _uow.GetGenericRepository<Distributor>()
                .FirstOrDefault(x => x.Id == product.DistributorId);

            var images = _uow.GetGenericRepository<ImageUri>().Find(x => x.ProductId == id).Take(4).ToList();
            var imageUris = new string[] { "", "", "", "" };

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




        //testing 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductEditCreateDto _product)
        {
            try
            {
                if (ModelState.IsValid)
                {

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


                    var repo = _uow.GetGenericRepository<ApplicationUser>();


                    var users = repo.GetAll() ;


                    //send email notification
                    var apiKey = _configuration.GetSection("SENDGRID_API_KEY").Value;
                    var client = new SendGridClient(apiKey);
                    var from = new EmailAddress("salesManager@example.com", "Sales Manager");


                  //  user DTO to get list of all users

                    List<EmailAddress> tos = new List<EmailAddress>();
                    foreach (var user in users)
                    {
                        tos.Add(new EmailAddress(user.Email)) ;

                    }

                   

                    var subject = "The Product" + product.Id.ToString() + "'s price has been updated to : " + product.Price.ToString();
                    var htmlContent = "<strong>Please check out our website with updated prices on products!</strong>";
                    var displayRecipients = false; // set this to true if you want recipients to see each others mail id 
                    var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, "", htmlContent, displayRecipients);
                    var response = client.SendEmailAsync(msg);
                  //var response = await client.SendEmailAsync(msg);

                    return RedirectToAction("Edit", new { id = id });
                }

                return View(); // error message should be returned.
            }
            catch
            {
                return View();
            }
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


        public IActionResult ShowGrid()
        {
            return View();
        }

        public IActionResult Orders()
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


        public async Task<JsonResult> LoadOrdersData()
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

                //get user id
                //var user = await _userManager.GetUserAsync(HttpContext.User);
                //var userId = user.Id;

                //var userRepo = _uow.GetGenericRepository<ApplicationUser>().Find(x => x.Id == userId).FirstOrDefault();
                // get entity data from context
                var data = (from tempData in _uow.GetGenericRepository<Order>().GetAll() select tempData);



                // Sorting
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    //.OrderBy(sortColumn + " " + sortColumnDirection).ToList();
                    var orderInfo = typeof(Order).GetProperty(sortColumn);
                    if (sortColumnDirection == "asc")
                    {
                        data = data.OrderBy(x => orderInfo.GetValue(x, null));
                    }
                    else if (sortColumnDirection == "desc")
                    {
                        data = data.OrderByDescending(x => orderInfo.GetValue(x, null));
                    }
                }
                // searching
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.Address.ToUpper().Contains(searchValue.ToUpper()));
                }

                //total number of rows count   
                recordsTotal = data.Count();

                //paging   
                var response = data.Skip(skip).Take(pageSize).ToList().Select(
                    x => new
                    {
                        id = x.Id,
                        user = x.CustomerId,
                        totalprice = x.TotalPrice,
                        address = x.Address,
                        isdelivered = x.IsDelivered

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
