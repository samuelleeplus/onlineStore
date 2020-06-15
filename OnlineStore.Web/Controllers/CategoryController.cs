using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        public IActionResult Index(string category="Index", string searchedProduct="", int pageNumber = 1, double minPrice = 0, double maxPrice = 1000000000)
        {
          
            // if category is not 'Index' store in session
            if (!category.Equals("Index"))
            {
                HttpContext.Session.SetString("category", category);
            }
            else
            {
                category = HttpContext.Session?.GetString("category");
            }


            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            const int pageSize = 12;

            /*
            var products = from m in _context.Products
                         select m;

            if (!String.IsNullOrEmpty(searchedProduct))
            {
                products = products.Where(s => s.Name.Contains(searchedProduct) && s.Price >= minPrice && s.Price <= maxPrice);
            }
            */

            var products = _uow.GetGenericRepository<Product>().Find(x => x.Category == category && x.DiscountedPrice >= minPrice && x.DiscountedPrice <= maxPrice && 
                                                                          (searchedProduct==null || x.Name.ToUpper().Contains(searchedProduct.ToUpper()))
                                                                          )?.ToList();


            var pages = new List<int>();
            int j = 0;
            for (int i = pageNumber; i*pageSize < products?.Count; i++)
            {
                j++;
                pages.Add(i);
                if (j == 3)
                {
                    break;
                }
            }
            
            var model = new CategoryDto()
            {
                Title = category,
                Text = "The Ultimate Driving Machine, amet consectetur adipisicing elit. " +
                       "Everything We Do is Driven By You corporis voluptatem sint Better Ideas Driven By You" +
                       "Unlike Any Other Engineered to Move the Human Spirit tenetur excepturi dolores unde. Repellat, dolorem.",

                BackgroundImageUrl = "https://car-images.bauersecure.com/pagefiles/68199/zmaser-001.jpg",


                Products = products?.Skip(12*(pageNumber-1)).Take(pageSize).Select(y =>
                    new SimpleProduct()
                    {
                     ImageUrl   = _uow.GetGenericRepository<ImageUri>().FirstOrDefault(z => z.ProductId == y.Id)?.Uri ?? null,
                     Name = y.Name,
                     OriginalPrice = y.Price,
                     DiscountedPrice = y.DiscountedPrice,
                     StatusClass = "Hot",
                     Url = "../../product?id=" + y.Id
                    }),
                Pages = pages
            };

            return View(model);
        }
        

    }
}