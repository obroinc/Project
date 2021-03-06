﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using ClubWestRFC.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubWestRFC.Pages.Members.Home
{
    public class IndexModel : PageModel
    {

        private readonly IUnitofWork _unitofWork;

        //constructor
        public IndexModel(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        //instead of putting in modelview, bind it here            

         public IEnumerable<Memberprice> MemberpriceList { get; set; }

        public IEnumerable<Category> CategoryList { get; set; }

        //to popluate the lists be able to filter/add properties using repository
        public void OnGet()
        {
            //check if user has ckecked in and retrieve the session and shopping cart count



            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
           
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim!=null)
            {
                int shoppingCartCount = _unitofWork.ShoppingCart.GetAll(a => a.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.ShoppingCart, shoppingCartCount);
             
            }



            MemberpriceList = _unitofWork.Memberprice.GetAll(null, null, "Category,MembershipType");

            CategoryList = _unitofWork.Category.GetAll(null, q => q.OrderBy(c => c.DisplayOrder), null);
        }
    }
}