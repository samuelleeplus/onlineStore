using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineStore.Data.Context;
using OnlineStore.Data.Repositories;
using OnlineStore.Web.Helpers;

namespace OnlineStore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _context { get; }
        private UnitOfWork _uow { get; }

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
            _uow = new UnitOfWork(_context);
        }

        public IActionResult Index(/*int amount=10, int frontAmount = 3*/)
        {
            try
            {
                var homeCtor = new DtoConstructor(_uow);

                var model = homeCtor.GetHomeDto(10, 3);

                var x = model;
                // throw new NullReferenceException("Just testing dude");
                return View(model);
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString());
                return View("ErrorProduction");

            }
        }
    }
}
