using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using ClubWestRFC.Models.ViewModels;
using ClubWestRFC.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubWestRFC.Pages.Members.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IUnitofWork _unitofWork;

        public IndexModel(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        public OrderDetailsCart OrderDetailsCartVM { get; set; }

        //add properties for OdercartDetailsVM
        public void OnGet()
        {
            OrderDetailsCartVM = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader(),
                listCart = new List<ShoppingCart>()
            };
            //set total to 0
            OrderDetailsCartVM.OrderHeader.OrderTotal = 0;

            //RETRIVING the shoppig cart of the user from the database , need Id of user
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                //reteriving items from shopping cart
                IEnumerable<ShoppingCart> cart = _unitofWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value);

                if (cart != null)
                {
                    OrderDetailsCartVM.listCart = cart.ToList();
                }
                //to calculate the order total
                foreach (var cartList in OrderDetailsCartVM.listCart)
                {
                    cartList.Memberprice = _unitofWork.Memberprice.GetFirstOrDefault(m => m.Id == cartList.MemberpriceId);
                    OrderDetailsCartVM.OrderHeader.OrderTotal += (cartList.Memberprice.Price * cartList.Count);
                }
            }
        }
        //to increment the no. of items in the cart
        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _unitofWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);
            _unitofWork.ShoppingCart.IncrementCount(cart, 1);
            _unitofWork.Save();
            return RedirectToPage("/Members/Cart/Index");
        }
        //to decrement the no. of items in the cart
        public IActionResult OnPostMinus(int cartId)
        {
            var cart = _unitofWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                _unitofWork.ShoppingCart.Remove(cart);
                _unitofWork.Save();

                var cnt = _unitofWork.ShoppingCart.
                                GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);
            }
            else
            {
                _unitofWork.ShoppingCart.DecrementCount(cart, 1);
                _unitofWork.Save();

            }


            return RedirectToPage("/Members/Cart/Index");
        }

        //to delete  the no. of items in the cart
        public IActionResult OnPostTrash(int cartId)
        {
            var cart = _unitofWork.ShoppingCart.GetFirstOrDefault(c => c.Id == cartId);
            _unitofWork.ShoppingCart.Remove(cart);
            _unitofWork.Save();

            var cnt = _unitofWork.ShoppingCart.
                               GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);
            return RedirectToPage("/Members/Cart/Index");
        }
    }
}
