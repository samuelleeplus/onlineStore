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
using OnlineStore.Web.Helpers;

namespace OnlineStore.Web.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext _context { get; }
        private UnitOfWork _uow { get; }

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
            _uow = new UnitOfWork(_context);
        }

        [HttpGet]
        public IActionResult Index(int id)
        {

            var prodCtor = new DtoConstructor(_uow);

            var model = prodCtor.GetProductDtoByProductId(id);

            var x = model;

            return View(model);
        }
    }
}