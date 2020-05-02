using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Web.Helpers;
using OnlineStore.Web.Models.DTOs;

namespace OnlineStore.Web.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private List<CartItem> _checkoutItems;

        public void LoadItems()
        {
            _checkoutItems = HttpContext?.Session.GetObjectFromJson<List<CartItem>>("cart") ?? null;
        }

        public IActionResult Index()
        {
            LoadItems();
            if (_checkoutItems == null)
            {
                return RedirectToRoute(new {controller = "Cart"});
            }

            LoadItems();

            var model = new CheckoutDto()
            {
                FirstName = "Taksim abi ne yapyon?",
                LastName = "Huhu",
                City = "Istanbul",
                AddressDetail = "Sabanci university",
                ZipCode = "34956",
                Province = "Tuzla",
                Phone = "+905555555555",
                Email = "tuzla@commi.org",
                IsRememberAddress = false,
                IsAgreeTermsAndConditions = false,
                CheckoutItems = _checkoutItems?.Select(x => new CheckoutItem()
                {
                    Price = x.ItemPrice * x.ItemQuantity,
                    ProductName = x.ItemName
                })
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CheckoutDto model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToRoute(new { controller="CreditCard" });
            }

            return View();
        }
    }
}