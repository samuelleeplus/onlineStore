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

            var model = new CartCartItemsDTO()
            {
                CartItems = new List<CartCartItem>(){
                    new CartItem()
                    {
                        CartItemImageUrl = "images/cart_1.jpg",
                        CartItemPrice = 790.90,
                        CartItemTotalPrice = 790.90,
                        CartItemQuantity = 2,
                        CartItemName = "Deluxe Cool and Great!"
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


        
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            var model = new CartItemsDto();
            
            if (cart != null)
            {
                model = new CartItemsDto()
                {
                    Items = cart
                };
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int id, int quantity)
        {
            var product = _uow.GetGenericRepository<Product>().GetById(id);
            if (SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart") == null)
            {
                List<CartItem> cart = new List<CartItem>();
                cart.Add(new CartItem {
                    Id = id,
                    ItemName = product.Name,
                    ItemQuantity = quantity,
                    ItemImageUrl = _uow.GetGenericRepository<ImageUri>().FirstOrDefault(x => x.ProductId == product.Id).Uri, //"google.com",  // product.ImageUris.ToList()[0].Uri,
                    ItemPrice = product.DiscountedPrice,
                    ItemTotalPrice = product.DiscountedPrice
                });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].ItemQuantity += quantity;
                }
                else
                {
                    cart.Add(new CartItem {
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

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            HttpContext.Session.Remove("cart");
            return Ok();
        }

        /*
        [Route("remove/{id}")]
        public IActionResult Remove(string id)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }
        */
        private int isExist(int id)
        {
            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");
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