using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Web.Helpers;
using OnlineStore.Web.Models.DTOs;

namespace OnlineStore.Web.Controllers
{
    public class CreditCardController : Controller
    {

        private List<CartItem> _checkoutItems;

        public void LoadItems()
        {
            _checkoutItems = HttpContext?.Session.GetObjectFromJson<List<CartItem>>("cart");
        }


        public IActionResult Index()
        {
            LoadItems();
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
        public IActionResult Index(string str)
        {
            if (ModelState.IsValid)
            {
                // TODO
                return RedirectToRoute(new {controller = "Home"});
            }

            return View();
        }

    }
}