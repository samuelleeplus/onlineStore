using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize]
    public class CheckoutController : Controller
    {
        private ILogger<CartController> _logger;
        private ApplicationDbContext _context;
        private UnitOfWork _uow;
        private UserManager<ApplicationUser> _userManager;

        public CheckoutController(ApplicationDbContext context, ILogger<CartController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _uow = new UnitOfWork(context);
            _userManager = userManager;
        }

        public async Task<IActionResult> ProceedToCheckout(int id)
        {

            List<CartItem> _checkoutItems = null;
            if (HttpContext?.Session != null)
                _checkoutItems = HttpContext?.Session.GetObjectFromJson<List<CartItem>>("cart");

            if (_checkoutItems == null)
            {
                return RedirectToRoute(new { controller = "Cart" });
            }

            if (id < 0)
            {
                return RedirectToAction("AddressCreateOrChoose");
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var addressList = _uow.GetGenericRepository<Address>().Find(x => x.UserId == user.CustomerId)?.ToList();


            if (addressList != null && addressList.Count > id)
            {
                SessionHelper.SetObjectAsJson(HttpContext.Session, "addressId", id);

                var address = addressList[id];
                var model = new CheckoutDto()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    AddressInfo = new AddressDto()
                    {
                        AddressDetail = address.AddressDetail,
                        City = address.City,
                        Province = address.Province,
                        ZipCode = address.ZipCode
                    },
                    Phone = user.PhoneNumber,
                    Email = user.Email,
                    IsAgreeTermsAndConditions = false,
                    CheckoutItems = _checkoutItems?.Select(x => new CheckoutItem()
                    {
                        Price = x.ItemPrice * x.ItemQuantity,
                        ProductName = x.ItemName
                    })
                };
                return View("Index", model);
            }
            return RedirectToAction("AddressCreateOrChoose");
        }
        [Route("[controller]")]
        [HttpGet]
        public async Task<IActionResult> AddressCreateOrChoose()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var addresses = _uow.GetGenericRepository<Address>().Find(x => x.UserId == user.CustomerId)?.
                Select(x => new AddressDto()
                {
                    AddressDetail = x.AddressDetail,
                    City = x.City,
                    ZipCode = x.ZipCode,
                    Province = x.Province
                });
            
            var model = new AddressDtoChooseOrCreate()
            {
                AddressDtoChoose = addresses
            };
            
            return View("Address", model);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutDto _)
        {
            if (ModelState.IsValid)
            {
                return RedirectToRoute(new { Controller="CreditCard", Action="ChooseCreditCardOrProceed" });
            }

            return RedirectToAction("AddressCreateOrChoose");
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAddress(AddressDtoChooseOrCreate arg) 
        {
            if (ModelState.IsValid)
            {
                var _address = arg.AddressDtoCreate;
                var addresses = _uow.GetGenericRepository<Address>();
                var user = await _userManager.GetUserAsync(HttpContext.User);
                addresses.Add(new Address
                {
                    UserId = user.CustomerId,
                    AddressDetail = _address.AddressDetail,
                    ZipCode = _address.ZipCode,
                    City = _address.City,
                    Province = _address.Province
                });
                _uow.Commit();
                return RedirectToAction("AddressCreateOrChoose");
            }
            return View("ErrorProduction");
        }
    }
}