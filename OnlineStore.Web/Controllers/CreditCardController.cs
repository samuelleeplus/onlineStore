using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Web.Controllers
{
    public class CreditCardController : Controller
    {
        public IActionResult Index()
        {
            return View();
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