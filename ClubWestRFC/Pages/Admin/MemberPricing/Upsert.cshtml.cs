using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubWestRFC.Pages.Admin.MemberPricing
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitofWork _unitofWork;
        private readonly IWebHostEnvironment _hostingenvironment;
        public UpsertModel(IUnitofWork unitofWork, IWebHostEnvironment hostingenvironment)
        {
            _unitofWork = unitofWork;
            _hostingenvironment = hostingenvironment;

        }

        [BindProperty]
        public MemberpriceVM MemberpriceObj { get; set; }


       //Get Handler
        public IActionResult OnGet(int? id)
        {
            MemberpriceObj = new MemberpriceVM
            //Drop down populated for select list dropdown
            {
                CategoryList = _unitofWork.Category.GetCategoryListForDropdown(),
                MembershipTypeList = _unitofWork.MembershipType.GetMembershipTypeListForDropdown(),

                //inniialistizing the Memberprice for the condidition
                //of (Model.MemberpriceObj.Memberprice.Id != 0) in the upsert.chtml 

                Memberprice = new Models.Memberprice()
            };

            //This is for an edit request
            if (id != null)
            {
            MemberpriceObj.Memberprice= _unitofWork.Memberprice.GetFirstOrDefault(u => u.Id == id);
                if (MemberpriceObj.Memberprice == null)
                {
                    return NotFound();
                }
            }
            return Page();

        }
        //Post Handler needs to save image uploaded bu admin user

        public IActionResult OnPost()
        {
            string webRootPath = _hostingenvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
        
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //adding a member item if the Id is 0
            if (MemberpriceObj.Memberprice.Id == 0)
            {
                //Changing to Guid
                string fileName = Guid.NewGuid().ToString();

                //Paths for uploads of pictures of different types of memberships
                var uploads = Path.Combine(webRootPath, @"images\MemberType");

                var extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                MemberpriceObj.Memberprice.image = @"\images\MemberType\" + fileName + extension;
               

                _unitofWork.Memberprice.Add(MemberpriceObj.Memberprice);
            }
            else
            {
                //Execute this only if we have to edit a memeber item

                var objFromDb = _unitofWork.Memberprice.Get(MemberpriceObj.Memberprice.Id);

                //CHECKING FILE COUNT, IF <0 NEW FILE HAS NOT BEEN UPLOADED, if >0 need to be able to
                //delete original file and create new one

                if (files.Count > 0)
                {

                    //Changing to Guid
                    string fileName = Guid.NewGuid().ToString();

                    //Paths for uploads of pictures of different types of memberships
                    var uploads = Path.Combine(webRootPath, @"images\MemberType");

                    var extension = Path.GetExtension(files[0].FileName);

                    //to get path of existing image

                    var imagePath = Path.Combine(webRootPath, objFromDb.image.TrimStart('\\'));

                    //delteing the orignil file to prepare for update
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    //To copy newfile
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    MemberpriceObj.Memberprice.image = @"\images\MemberType\" + fileName + extension;

                    //not needed as its inside edit
                    //_unitofWork.Memberprice.Add(MemberpriceObj.Memberprice);

                }
                else
                {
                    MemberpriceObj.Memberprice.image = objFromDb.image;
                }

                _unitofWork.Memberprice.Update(MemberpriceObj.Memberprice);
            }
            _unitofWork.Save();
            return RedirectToPage("./Index");
        }
    }
}