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
                Text = "Lorem ipsum dolor sit, amet consectetur adipisicing elit. " +
                       "Consectetur quia perferendis vitae corporis voluptatem sint dolorum rem. Distinctio " +
                       "laboriosam aspernatur ipsum atque blanditiis quo tenetur excepturi dolores unde. Repellat, dolorem.",

                BackgroundImageUrl = "https://cdn.webtekno.com/media/cache/content_detail_v2/article/83841/internet-fenomeni-taksim-dayi-hayatinda-ilk-kez-taksim-e-geldi-1579253407.png",

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