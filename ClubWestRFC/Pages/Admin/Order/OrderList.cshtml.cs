using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubWestRFC.Pages.Admin.OrderList
{
    public class OrderListModel : PageModel
    {

        public IActionResult OnGet()
        {
            return Page();
        }

    }
}