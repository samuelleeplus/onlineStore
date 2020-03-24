using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using OnlineStore.Data.Models.Entities;

namespace OnlineStore.Web.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Products()
        {
            var product = new Product()
            {
                Id = 1,
                AppliedDiscount = 0,
                Description = "test description",
                Name = "The first product",
                Price = 12.34f
            };

            return View(product);
        }
    }
}