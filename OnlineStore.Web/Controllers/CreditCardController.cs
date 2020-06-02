using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineStore.Data.Context;
using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Repositories;
using OnlineStore.Web.Helpers;
using OnlineStore.Web.Models.DTOs;

namespace OnlineStore.Web.Controllers
{
    public class CreditCardController : Controller
    {

        private ILogger<CartController> _logger;
        private ApplicationDbContext _context;
        private UnitOfWork _uow;
        private UserManager<ApplicationUser> _userManager;


        public CreditCardController(ApplicationDbContext context, ILogger<CartController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _uow = new UnitOfWork(context);
            _userManager = userManager;
        }
        

        public IActionResult Index()
        {
            List<CartItem> _checkoutItems = null;
            if (HttpContext?.Session != null)
                _checkoutItems = HttpContext?.Session.GetObjectFromJson<List<CartItem>>("cart");

            if (_checkoutItems != null)
            {
                var model = new CreditCardDto()
                {
                    CheckoutItems = _checkoutItems?.Select(x => new CheckoutItem()
                    {
                        Price = x.ItemPrice * x.ItemQuantity,
                        ProductName = x.ItemName
                    })
                };
                return View(model);

            }

            return RedirectToRoute(new {controller = "Cart"});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CreditCardDto dto)
        {
            if (ModelState.IsValid)
            {

                // 
                int addressId = -1;
                if (HttpContext?.Session != null)
                    addressId = SessionHelper.GetObjectFromJson<int>(HttpContext.Session, "addressId");
                else
                {
                    return View("ErrorProduction");
                }


                var user = await _userManager.GetUserAsync(HttpContext.User);
                var address = _uow.GetGenericRepository<Address>().Find(x => x.UserId == user.CustomerId)?.ToList()[addressId];
                var _checkoutItems = HttpContext?.Session.GetObjectFromJson<List<CartItem>>("cart");
                if (_checkoutItems == null)
                {
                    return View("ErrorProduction");
                }

                Order order = new Order()
                {
                    Address = $"{address.AddressDetail}, {address.Province}, {address.City}, {address.ZipCode}",
                    CustomerId = user.CustomerId,
                    IsDelivered = false,
                    TotalPrice = _checkoutItems.Select(x => x.ItemPrice*x.ItemQuantity).Aggregate((x,y) => x+y)
                };

                _uow.GetGenericRepository<Order>().Add(order);
                _uow.Commit();

                var orderedProductRepository = _uow.GetGenericRepository<OrderedProduct>();
                var productRepository = _uow.GetGenericRepository<Product>();

                _checkoutItems.ForEach(x =>
                {
                    orderedProductRepository.Add( new OrderedProduct()
                    {
                        OrderId = order.Id,
                        ProductId = x.Id,
                        Quantity = x.ItemQuantity
                    });
                    
                    var product = productRepository.FirstOrDefault(y => y.Id == x.Id);
                    product.Quantity -= x.ItemQuantity;
                    productRepository.Update(product);
                    
                });
                
                _uow.Commit();

                HttpContext.Session.Remove("cart");
                HttpContext.Session.Remove("addressId");
                // return RedirectToRoute(new {controller = "Home"});
                return View("SuccessMessage");
            }

            return View();
        }

    }
}