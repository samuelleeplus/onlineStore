using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data.Context;
using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Repositories;
using OnlineStore.Web.Models.DTOs;

namespace OnlineStore.Web.Controllers
{
    public class CategoryController : Controller
    {

        private ApplicationDbContext _context { get; }
        private UnitOfWork _uow { get; }

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
            _uow = new UnitOfWork(_context);
        }
        // [Route("/articles/{page}")]
        [Route("[controller]/{category}")]
        public IActionResult Index(string category)
        {

            var model = new CategoryDto()
            {
                Title = category,
                Text = "The Ultimate Driving Machine, amet consectetur adipisicing elit. " +
                       "Everything We Do is Driven By You corporis voluptatem sint Better Ideas Driven By You" +
                       "Unlike Any Other Engineered to Move the Human Spirit tenetur excepturi dolores unde. Repellat, dolorem.",

                BackgroundImageUrl = "https://car-images.bauersecure.com/pagefiles/68199/zmaser-001.jpg",

                Products = _uow.GetGenericRepository<Product>().Find(x => x.Category == category).Select(y =>
                    new SimpleProduct()
                    {
                     ImageUrl   = _uow.GetGenericRepository<ImageUri>().FirstOrDefault(z => z.ProductId == y.Id)?.Uri ?? null,
                     Name = y.Name,
                     OriginalPrice = y.Price,
                     DiscountedPrice = y.DiscountedPrice,
                     StatusClass = "Hot",
                     Url = "../../product?id=" + y.Id
                    })
            };

            return View(model);
        }
        

    }
}