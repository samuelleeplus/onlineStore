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
                    new Category{ CategoryName = "Category11", CategoryLink=""  },
                    new Category{ CategoryName = "Category22", CategoryLink=""  },
                    new Category{ CategoryName = "Category33", CategoryLink=""  },
                    new Category{ CategoryName = "Category44", CategoryLink=""  },
                    new Category{ CategoryName = "Category55", CategoryLink=""  }
                }
            };
            return PartialView(model);
        }
    }
}