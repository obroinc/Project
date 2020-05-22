using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubWestRFC.Pages.Admin.MemberShipType
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitofWork _unitofWork;

        public UpsertModel(IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
        }

        [BindProperty]
        public Models.MembershipType MembershipTypeObj { get; set; }

        public IActionResult OnGet(int? id)
        {
            MembershipTypeObj = new Models.MembershipType();
            if (id != null)
            {
                MembershipTypeObj = _unitofWork.MembershipType.GetFirstOrDefault(u => u.Id == id);
                if (MembershipTypeObj == null)
                {
                    return NotFound();
                }
            }
            return Page();

        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (MembershipTypeObj.Id == 0)
            {
                _unitofWork.MembershipType.Add(MembershipTypeObj);
            }
            else
            {
                _unitofWork.MembershipType.Update(MembershipTypeObj);
            }
            _unitofWork.Save();
            return RedirectToPage("./Index");
        }
    }
}