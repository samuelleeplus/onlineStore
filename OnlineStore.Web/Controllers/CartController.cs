using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data.Context;
using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Repositories;
using OnlineStore.Web.Helpers;
using OnlineStore.Web.Models.DTOs;

namespace OnlineStore.Web.Controllers
{
    public class CartController : Controller
    {
        /*
        public IActionResult Index()
        {

            var model = new CartCartITemsDTO()
            {
                CartITems = new List<CartCartITem>(){
                    new CartITem()
                    {
                        CartITemImageUrl = "images/cart_1.jpg",
                        CartITemPrice = 790.90,
                        CartITemTotalPrice = 790.90,
                        CartITemQuantity = 2,
                        CartITemName = "Deluxe Cool and Great!"
                    }
                }
            };

            return View(model);
        }

        */

        private ApplicationDbContext _context { get; set; }
        private UnitOfWork _uow { get; set; }
        
        public CartController(ApplicationDbContext context)
        {
            _context = context;
            _uow = new UnitOfWork(context);
        }


        [Route("index")]
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartITem>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(CartITem => CartITem.ItemPrice * CartITem.ItemQuantity);
            return View();
        }

        [HttpPost]
        public IActionResult Buy(int id)
        {
            var product = _uow.GetGenericRepository<Product>().GetById(id);
            if (SessionHelper.GetObjectFromJson<List<CartITem>>(HttpContext.Session, "cart") == null)
            {
                List<CartITem> cart = new List<CartITem>();
                cart.Add(new CartITem {
                    Id = id,
                    ItemName = product.Name,
                    ItemQuantity = 1,
                    ItemImageUrl = "google.com",  // product.ImageUris.ToList()[0].Uri,
                    ItemPrice = product.DiscountedPrice,
                    ItemTotalPrice = product.DiscountedPrice
                });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<CartITem> cart = SessionHelper.GetObjectFromJson<List<CartITem>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].ItemQuantity++;
                }
                else
                {
                    cart.Add(new CartITem {
                        ItemName = product.Name,
                        ItemQuantity = 1,
                        ItemImageUrl = null, //product.ImageUris.ToList()[0].Uri,
                        ItemPrice = product.DiscountedPrice,
                        ItemTotalPrice = product.DiscountedPrice
                    });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return Ok();
         }
        /*
        [Route("remove/{id}")]
        public IActionResult Remove(string id)
        {
            List<CartITem> cart = SessionHelper.GetObjectFromJson<List<CartITem>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }
        */
        private int isExist(int id)
        {
            List<CartITem> cart = SessionHelper.GetObjectFromJson<List<CartITem>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Id.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

    }
}