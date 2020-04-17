using Microsoft.AspNetCore.Mvc;
using OnlineStore.Web.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineStore.Web.Helpers;

namespace OnlineStore.Web.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cartItems = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");

            var model = new NavigationDto
            {
                Categories = new List<Category>
                {
                    new Category{ CategoryName = "Category1", CategoryLink=""  },
                    new Category{ CategoryName = "Category2", CategoryLink=""  },
                    new Category{ CategoryName = "Category3", CategoryLink=""  },
                    new Category{ CategoryName = "Category4", CategoryLink=""  },
                    new Category{ CategoryName = "Category5", CategoryLink=""  }
                },
                NumberOfItemsInCart = cartItems?.Select(x => x.ItemQuantity).Aggregate((x, y) => x+y) ?? 0
            };
            return View(model);
        }
    }
}
