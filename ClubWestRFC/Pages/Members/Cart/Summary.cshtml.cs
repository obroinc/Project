using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using ClubWestRFC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubWestRFC.Pages.Members.Cart
{
    public class SummaryModel : PageModel
    {
        private readonly IUnitofWork _unitofWork;

        public SummaryModel(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }
        [BindProperty]
        public OrderDetailsCart detailsCart  { get; set; }

        //add properties for OdercartDetailsVM
        public IActionResult OnGet()
        {
            detailsCart = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader()
            };

            detailsCart.OrderHeader.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<ShoppingCart> cart = _unitofWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value);

            if (cart != null)
            {
                detailsCart.listCart = cart.ToList();
            }

            foreach (var cartList in detailsCart.listCart)
            {
                cartList.Memberprice = _unitofWork.Memberprice.GetFirstOrDefault(m => m.Id == cartList.MemberpriceId);
                detailsCart.OrderHeader.OrderTotal += (cartList.Memberprice.Price * cartList.Count);
            }

            ApplicationUser applicationUser = _unitofWork.ApplicationUser.GetFirstOrDefault(c => c.Id == claim.Value);
            detailsCart.OrderHeader.PickupName = applicationUser.FullName;
            detailsCart.OrderHeader.PickUpTime = DateTime.Now;
            detailsCart.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            return Page();

        }

    }

    
}