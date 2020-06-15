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
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Web.Controllers
{
    public class ProductController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context { get; }
        private UnitOfWork _uow { get; }

        public ProductController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _uow = new UnitOfWork(_context);
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            var prodCtor = new DtoConstructor(_uow);

            var model = prodCtor.GetProductDtoByProductId(id);

            var x = model;

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview([FromForm]int star, [FromForm]string review, int id)
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var reviewRepository = _uow.GetGenericRepository<Review>();
            reviewRepository.Add(new Review()
            {
                CustomerId = user.CustomerId,
                ProductId = id,
                StarNum = star,
                ReviewOfProduct = review
            });
            _uow.Commit();

            return Ok();
        }


        public IActionResult SeeReviews(int productId)
        {
            var repo = _uow.GetGenericRepository<Review>();
            var model = repo.Find(x => x.ProductId == productId).Select(y => new ReviewDto()
            {
                Rating = y.StarNum,
                Review = y.ReviewOfProduct
            });
            return View(model);
        }

    }
}