using Microsoft.AspNetCore.Mvc;
using OnlineStore.Web.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Web.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
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
                NumberOfItemsInCart = 0
            };
            return View(model);
        }
    }
}
