using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using ClubWestRFC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubWestRFC.Pages.Members.Home
{
    [Authorize]
    public class DetailsModel : PageModel

    {
        private readonly IUnitofWork _unitofWork;

        public DetailsModel (IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }


        [BindProperty]
        public ShoppingCart ShoppingCartObj { get; set;}


        public void OnGet(int id)
        {
            ShoppingCartObj = new ShoppingCart()
            {
                Memberprice = _unitofWork.Memberprice.GetFirstOrDefault(includeProperties: "Category,MembershipType", filter: c=>c.Id==id),
                MemberpriceId = id
            };
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                //reteive user id that is logged in
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                //Application id inside shopping cart with its properties
                ShoppingCartObj.ApplicationUserId = claim.Value;

                //to retireve items from database using allpicaionuser and Memgeprice Ids
                ShoppingCart cartFromDb = _unitofWork.ShoppingCart.GetFirstOrDefault(c => c.ApplicationUserId == ShoppingCartObj.ApplicationUserId &&
                  c.MemberpriceId == ShoppingCartObj.MemberpriceId);


                //Checking to see if there is anything in the shopping cart

            if(cartFromDb==null)
                {
                    _unitofWork.ShoppingCart.Add(ShoppingCartObj);
                }
                else
                {

                    cartFromDb.Count = _unitofWork.ShoppingCart.IncrementCount(cartFromDb, ShoppingCartObj.Count);

                }
                _unitofWork.Save();
                //to increase the session
                var count = _unitofWork.ShoppingCart.GetAll(c => c.ApplicationUserId == ShoppingCartObj.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ShoppingCart, count);

                return RedirectToPage("Index");
            }
            else
            {
                ShoppingCartObj.Memberprice = _unitofWork.Memberprice.GetFirstOrDefault(includeProperties: "Category,MembershipType", filter: c => c.Id == ShoppingCartObj.MemberpriceId);
              

                //return to the page itself
                return Page();
            }

            

        }
    }
}