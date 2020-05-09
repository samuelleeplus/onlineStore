﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data.Context;
using OnlineStore.Data.Models.Entities;
using OnlineStore.Data.Repositories;
using OnlineStore.Web.Helpers;
using OnlineStore.Web.Models.DTOs;
using System.Linq;

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
                    temp.Country = address.Country;
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


    }
}