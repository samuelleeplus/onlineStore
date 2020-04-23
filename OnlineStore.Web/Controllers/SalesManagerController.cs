﻿using System;
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
            return RedirectToAction("ListProducts");
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
    }
}