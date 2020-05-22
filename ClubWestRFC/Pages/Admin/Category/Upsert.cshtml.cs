using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubWestRFC.Pages.Admin.Category
{
    public class UpsertModel : PageModel
    {
        
            private readonly IUnitofWork _unitofWork;

            public UpsertModel(IUnitofWork unitofWork)
            {
                _unitofWork = unitofWork;
            }

            [BindProperty]
            public Models.Category CategoryObj { get; set; }

            public IActionResult OnGet(int? id)
            {
                CategoryObj = new Models.Category();
                if (id != null)
                {
                    CategoryObj = _unitofWork.Category.GetFirstOrDefault(u => u.Id == id);
                    if (CategoryObj == null)
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
                if (CategoryObj.Id == 0)
                {
                    _unitofWork.Category.Add(CategoryObj);
                }
                else
                {
                    _unitofWork.Category.Update(CategoryObj);
                }
                _unitofWork.Save();
                return RedirectToPage("./Index");
            }
        }
    }