using Microsoft.AspNetCore.Mvc;
using OnlineStore.Web.Models.DTOs;
using System.Collections.Generic;
using System.Linq;
using OnlineStore.Data.Context;
using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Repositories;
using OnlineStore.Web.Helpers;
using OnlineStore.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Web.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private ApplicationDbContext _context { get; }
        private UnitOfWork _uow { get; }

        private UserManager<ApplicationUser> _userManager;

        public NavigationViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _uow = new UnitOfWork(_context);
            _userManager = userManager; 
        }


        public async System.Threading.Tasks.Task<IViewComponentResult> InvokeAsync()
        {
            var cartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var model = new NavigationDto
            {
                Categories = _uow.GetGenericRepository<Product>().GetAll().Select(x => x.Category).ToHashSet().Select(
                    x => 
                        new Category{ CategoryName = x, CategoryLink = "/category/" + x}
                    ).ToList(),

                NumberOfItemsInCart = cartItems?.Select(x => x.ItemQuantity).Aggregate((x, y) => x+y) ?? 0,
                 Username = user?.FirstName
            };
            return View(model);
        }
    }
}
