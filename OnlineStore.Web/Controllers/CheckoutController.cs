using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Web.Helpers;
using OnlineStore.Web.Models.DTOs;

namespace OnlineStore.Web.Controllers
{
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

            var model = new CheckoutDto()
            {
                FirstName = "Hehe",
                LastName = "Huhu",
                City = "Istanbul",
                AddressDetail = "Sabanci university",
                ZipCode = "34956",
                Province = "Tuzla",
                Phone = "+905555555555",
                Email = "tuzla@commi.org",
                IsRememberAddress = true,
                IsAgreeTermsAndConditions = false

            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(CheckoutDto model)
        {
            return View("ErrorProduction");
        }
    }
}