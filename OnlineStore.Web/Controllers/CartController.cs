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
                    ItemImageUrl = _uow.GetGenericRepository<ImageUri>().FirstOrDefault(x => x.ProductId == product.Id)?.Uri,
                    ItemPrice = product.DiscountedPrice,
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
                        Id = id,
                        ItemName = product.Name,
                        ItemQuantity = quantity,
                        ItemImageUrl = _uow.GetGenericRepository<ImageUri>().FirstOrDefault(x => x.ProductId == product.Id)?.Uri,
                        ItemPrice = product.DiscountedPrice,
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


        // update?id_and_counts
        [HttpPost]
        public async Task<IActionResult> Update([FromQuery]string id_and_counts)
        {

            // "1, 2, 3, 4; 12, 1, 2, 3"
            var tuple = id_and_counts.Split(";");
            var ids = tuple[0].Split(",").Select(x => Int32.Parse(x)).ToList();

            var counts = tuple[1].Split(",").Select(x => Int32.Parse(x)).ToList();


            List<CartItem> cart = SessionHelper.GetObjectFromJson<List<CartItem>>(HttpContext.Session, "cart");

            for (int i = 0; i < ids.Count; i++)
            {
                var entry = cart.Find(x => x.Id == ids[i]);
                if (entry != null)
                {
                    if (counts[i] == 0)
                    {
                        cart.Remove(entry);
                    }
                    else
                    {
                        entry.ItemQuantity = counts[i];
                    }
                }
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("cart");
            }

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