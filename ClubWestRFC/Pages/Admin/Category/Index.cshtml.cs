using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubWestRFC.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubWestRFC.Pages.Admin.Category
{

    //Authorise Admin to have access to this page only
    [Authorize(Roles = SD.AdminRole)]

    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}