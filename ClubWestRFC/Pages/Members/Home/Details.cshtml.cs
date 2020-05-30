using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubWestRFC.Pages.Members.Home
{
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
                Memberprice = _unitofWork.Memberprice.GetFirstOrDefault(includeProperties: "Category,MemberShipType", filter: c=>c.Id==id),
                MemberpriceId = id
            };
        }
    }
}