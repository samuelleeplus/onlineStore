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
        public ApplicationDbContext _context { get; set; }
        public UnitOfWork _uow { get; set; }

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
            _uow = new UnitOfWork(_context);
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            var product = _uow.GetGenericRepository<Product>().GetById(id);

            var model = new ProductDTO
            {
                Name = product.Name,
                Description = product.DescriptionMain,
                DescriptionExtra = product.DescriptionExtra,
                OriginalPrice = product.Price,
                DiscountedPrice = product.DiscountedPrice
            };
            return View(model);
        }
    }
}