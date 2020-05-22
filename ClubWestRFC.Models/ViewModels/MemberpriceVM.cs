using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClubWestRFC.Models.ViewModels
{
    public class MemberpriceVM
    {
        public Memberprice Memberprice { get; set; }

        //For  drop down menu
        public IEnumerable<SelectListItem> CategoryList{ get; set; }

        public IEnumerable<SelectListItem> MembershipTypeList { get; set; }
    }
}
