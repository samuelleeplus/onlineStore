using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using OnlineStore.Data.Context;
using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Repositories;
using OnlineStore.Web.Helpers;
using OnlineStore.Web.Models.DTOs;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.Web.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext _context { get; set; }
        private UnitOfWork _uow { get; set; }
        private UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _uow = new UnitOfWork(_context);
            _userManager = userManager;
        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> IndexAsync()
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var userId = user.Id;

            var prodCtor = new DtoConstructor(_uow);

            var model = prodCtor.UserDtoByCustomerId(userId);

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, UserDto user)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var userRepo = _uow.GetGenericRepository<ApplicationUser>();

                    var temp = userRepo.Find(x => x.Id == user.UserId).FirstOrDefault();

                    var user1 = _userManager.FindByIdAsync(id);


                    temp.FirstName = user.FirstName;
                    temp.LastName = user.LastName;
                    temp.PhoneNumber = user.PhoneNumber;
                    temp.Email = user.Email;
                    userRepo.Update(temp);
                    _uow.Commit();

                    return RedirectToAction("Index");
                }

                return View(); // error message should be returned.
            }
            catch
            {
                return View();
            }
        }

        //addresses 
        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> AddressAsync()
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var userId = user.Id;

            var prodCtor = new DtoConstructor(_uow);

            var model = prodCtor.AddressDtoByCustomerId(userId);

            return View(model);

        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> AddressDetail(int addressId)
        {
            var addressRepo = _uow.GetGenericRepository<Address>();

            var temp = addressRepo.Find(x => x.Id == addressId).FirstOrDefault();

            return View(temp); // error message should be returned.

        }


        public ActionResult AddressEdit(Address address)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var addressRepo = _uow.GetGenericRepository<Address>();

                    var temp = addressRepo.Find(x => x.Id == address.Id).FirstOrDefault();

                    temp.AddressDetail = address.AddressDetail;
                    temp.City = address.City;
                    temp.Province = address.Province;
                    temp.ZipCode = address.ZipCode;

                    addressRepo.Update(temp);
                    _uow.Commit();

                    return RedirectToAction("Address");
                }

                return View("AddressDetail"); // error message should be returned.
            }
            catch
            {
                return View();
            }
        }



        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> CreditCardAsync()
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);

            var userId = user.Id;

            var prodCtor = new DtoConstructor(_uow);

            var model = prodCtor.CreditCardDtoByCustomerId(userId);

            return View(model);

        }

        [HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> CreditCardDetail(int cardId )
        {
            var cardRepo = _uow.GetGenericRepository<CreditCard>();

            var temp = cardRepo.Find(x => x.Id == cardId).FirstOrDefault();

            var dto = new CreditCardDto()
            {
                CardNumber = temp.CardNumber,
                Cvc = temp.Cvc,
                FullName = temp.FullName,
                ExpiryDate = temp.ExpiryDate


            };

            return View(dto); // error message should be returned.

        }

        public async System.Threading.Tasks.Task<ActionResult> CreditCardEditAsync(CreditCardDto card)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var cardRepo = _uow.GetGenericRepository<CreditCard>();

                    var user = await _userManager.GetUserAsync(HttpContext.User);



                    //???
                    //should we be finding by cardID? e.g. single customer may have many cards
                    var temp = cardRepo.Find(x => x.CustomerId == user.CustomerId).FirstOrDefault();

                    temp.CardNumber = card.CardNumber;
                    temp.Cvc = card.Cvc;
                    temp.ExpiryDate = card.ExpiryDate;
                    temp.FullName = card.FullName;

                    cardRepo.Update(temp);
                    _uow.Commit();

                    return RedirectToAction("CreditCard");
                }

                return View("CreditCardDetail"); // error message should be returned.
            }
            catch
            {
                return View();
            }
        }

        public IActionResult Orders()
        {
            return View();
        }
        public async Task<JsonResult> LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                // Skiping number of Rows count  
                var start = Request.Query["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Query["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Query["columns[" + Request.Query["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Query["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                //get user id
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var userId = user.Id;

                var userRepo = _uow.GetGenericRepository<ApplicationUser>().Find(x => x.Id == userId).FirstOrDefault();

                var data = _uow.GetGenericRepository<Order>().Find(x => x.CustomerId == userRepo.CustomerId);
                // get entity data from context
                //var data = (from tempData in _uow.GetGenericRepository<Order>().GetAll() select tempData);



                // Sorting
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
                {
                    //.OrderBy(sortColumn + " " + sortColumnDirection).ToList();
                    var orderInfo = typeof(Order).GetProperty(sortColumn);
                    if (sortColumnDirection == "asc")
                    {
                        data = data.OrderBy(x => orderInfo.GetValue(x, null));
                    }
                    else if (sortColumnDirection == "desc")
                    {
                        data = data.OrderByDescending(x => orderInfo.GetValue(x, null));
                    }
                }
               // searching
               //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    data = data.Where(m => m.Address.ToUpper().Contains(searchValue.ToUpper()));
                }

                //total number of rows count   
                recordsTotal = data.Count();

                //paging   
                var response = data.Skip(skip).Take(pageSize).ToList().Select(
                    x => new
                    {
                        id = x.Id,
                        user = x.CustomerId,
                        totalprice = x.TotalPrice,
                        address = x.Address,
                        isdelivered = x.IsDelivered

                    }).ToList();

                //Returning Json Data  
                return new JsonResult(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = response });
            }
            catch (Exception)
            {
                throw;
            }


        }





    }
}