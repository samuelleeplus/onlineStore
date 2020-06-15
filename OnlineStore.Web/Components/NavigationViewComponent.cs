using Microsoft.AspNetCore.Mvc;
using OnlineStore.Web.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Data.Context;
using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Repositories;
using OnlineStore.Web.Helpers;

namespace OnlineStore.Web.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private ApplicationDbContext _context { get; }
        private UnitOfWork _uow { get; }

        public NavigationViewComponent(ApplicationDbContext context)
        {
            _context = context;
            _uow = new UnitOfWork(_context);
        }


        public IViewComponentResult Invoke()
        {
            var cartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");

            var model = new NavigationDto
            {
                Categories = _uow.GetGenericRepository<Product>().GetAll().Select(x => x.Category).ToHashSet().Select(
                    x => 
                        new Category{ CategoryName = x, CategoryLink = "/category/" + x}
                    ).ToList(),

                NumberOfItemsInCart = cartItems?.Select(x => x.ItemQuantity).Aggregate((x, y) => x+y) ?? 0
            };
            return View(model);
        }
    }
}
