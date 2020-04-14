using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Web.Models.DTOs;

namespace OnlineStore.Web.Controllers
{
    public class NavigationController : Controller
    {
        public IActionResult Index()
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
                }
            };
            return PartialView(model);
        }
    }
}