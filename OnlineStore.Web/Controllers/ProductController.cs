using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Context;
using OnlineStore.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Web.Models.DTOs;

namespace OnlineStore.Web.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext _context { get; set; }
        private UnitOfWork _uow { get; set; }

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
            _uow = new UnitOfWork(_context);
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            var repo = _uow.GetGenericRepository<Product>();
            var product = repo.GetById(1);


            var _relatedProducts = repo.Find(x => x.Category == product.Category && x.Id != product.Id).Take(5).ToList();
            var temp = product.ImageUris;
            var select = temp == null ? null : temp.Select(x => x.Uri);
            var imageUrls = select == null ? null : select.ToList();



            /*
            var ImageUris = _context.ImageUris;
            var queryBackgroundImageUrl = from uri in ImageUris
                                          where uri.ProductId == id
                                          select uri.Uri;
            var imageUrls = queryBackgroundImageUrl.AsEnumerable();
            var productList = _uow.GetGenericRepository<Product>().GetAll().ToList(); // when we do this the Distributer and ImageUri are null make a code where it joins all of it
            var relatedProductList = new List<RelatedProduct>();
            foreach (var randomProduct in productList)
            {
                var relatedProduct = new RelatedProduct
                {
                    Url = "https://localhost:44396/", // we should get the url for this product so like localhost/product/id
                    ImageUrl = "https://openimagedenoise.github.io/images/moana_16spp_oidn.jpg", //randomProduct.ImageUris.First().Uri,
                    Name = randomProduct.Name,
                    Price = randomProduct.Price, // Price or Discounted Price?
                    StatusClass = "Yes?" // I dont know what statusclass is supposed to be
                };
                relatedProductList.Append(relatedProduct);
            }
            */
            var header = new Header
            {
                BackgroundImageUrl = "https://openimagedenoise.github.io/images/moana_16spp_oidn.jpg",
                Title = product.Name,
                Text = product.DescriptionMain
            };

            var relatedProducts = _relatedProducts.Select(x =>
                new RelatedProduct
                {
                    ImageUrl = "https://openimagedenoise.github.io/images/moana_16spp_oidn.jpg", // x.ImageUris.Take(1).Select(y => y.Uri).FirstOrDefault(),
                    Url = "https://localhost:44396/",
                    Price = x.DiscountedPrice,
                    StatusClass = "New"
                }).ToList();


            var model = new ProductDTO
            {
                Header = header,
                Name = product.Name,
                Description = product.DescriptionMain,
                DescriptionExtra = product.DescriptionExtra,
                ImageUrls = imageUrls,
                OriginalPrice = product.Price,
                DiscountedPrice = product.DiscountedPrice,
                RelatedProducts = relatedProducts
            };
            if (product.Quantity <= 0)
            {
                model.Availability = "Out of Stock";
            }

            return View(model);
        }
    }
}