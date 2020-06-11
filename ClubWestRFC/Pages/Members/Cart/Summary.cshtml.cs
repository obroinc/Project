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
using Stripe;

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



        public IActionResult OnPost(string stripeToken)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            detailsCart.listCart = _unitofWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).ToList();

            detailsCart.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            detailsCart.OrderHeader.OrderDate = DateTime.Now;
            detailsCart.OrderHeader.UserId = claim.Value;
            detailsCart.OrderHeader.Status = SD.PaymentStatusPending;
            detailsCart.OrderHeader.PickUpTime = Convert.ToDateTime(detailsCart.OrderHeader.PickUpDate.ToShortDateString() + " " + detailsCart.OrderHeader.PickUpTime.ToShortTimeString());

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            _unitofWork.OrderHeader.Add(detailsCart.OrderHeader);
            _unitofWork.Save();

            foreach (var item in detailsCart.listCart)
            {
                item.Memberprice = _unitofWork.Memberprice.GetFirstOrDefault(m => m.Id == item.MemberpriceId);
               
                OrderDetails orderDetails = new OrderDetails
                {
                    MemberpriceId = item.MemberpriceId,
                    OrderId = detailsCart.OrderHeader.Id,
                    Description = item.Memberprice.Description,
                    Name=item.Memberprice.Name,
                    Price = item.Memberprice.Price,
                    Count = item.Count
                };

                detailsCart.OrderHeader.OrderTotal += (orderDetails.Count * orderDetails.Price);
                _unitofWork.OrderDetails.Add(orderDetails);

            }

            detailsCart.OrderHeader.OrderTotal = Convert.ToDouble(String.Format("{0:.##}", detailsCart.OrderHeader.OrderTotal));
            _unitofWork.ShoppingCart.RemoveRange(detailsCart.listCart);
            HttpContext.Session.SetInt32(SD.ShoppingCart, 0);
            _unitofWork.Save();

            if (stripeToken != null)
            {

                var options = new ChargeCreateOptions
                {
                    //Amount is in cents so multiply by 100
                    Amount = Convert.ToInt32(detailsCart.OrderHeader.OrderTotal * 100),
                    Currency = "eur",
                    Description = "Order ID : " + detailsCart.OrderHeader.Id,
                    Source = stripeToken
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);

                detailsCart.OrderHeader.TransactionId = charge.Id;

                if (charge.Status.ToLower() == "succeeded")
                {
                    //email 
                    detailsCart.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    detailsCart.OrderHeader.Status = SD.StatusSubmitted;
                }
                else
                {
                    detailsCart.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                }
            }
            else
            {
                detailsCart.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
            }
            _unitofWork.Save();

            return RedirectToPage("/Members/Cart/OrderConfirmation", new { id = detailsCart.OrderHeader.Id });

        }



    }


}

    
