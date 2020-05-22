using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClubWestRFC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberpriceController : Controller
    {

        private readonly IUnitofWork _unitofwork;

        //To be deleting an item we delete the images from the server

        private readonly IWebHostEnvironment _hostingenvironment;

        //need to add in the startup.cs  services.AddScoped<IUnitofWork, UnitofWork>();
        //Need unit of work allows to access the database

        public MemberpriceController(IUnitofWork unitofwork, IWebHostEnvironment hostingenvironment)
        {
            _unitofwork = unitofwork;

            _hostingenvironment = hostingenvironment;
        }

        //Retreiving all of the categories and return back
        [HttpGet]
        public IActionResult Get()
        {
            //Category and Membershiptype Id will be included here asthey are paramneters set in Repository GetAll
             return Json(new { data = _unitofwork.Memberprice.GetAll(null,null,"Category,MembershipType") });
        }
        //for Delete
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
                    {
            try
            {

            
             var objFromDb = _unitofwork.Memberprice.GetFirstOrDefault(u => u.Id == Id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Someting went wrong with deleting" });
            }

            //to check if in image exits for it be deleted

            //to get image path from the server BY navigating to root folder
            var imagePath = Path.Combine(_hostingenvironment.WebRootPath, objFromDb.image.TrimStart('\\'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

             _unitofwork.Memberprice.Remove(objFromDb);
            _unitofwork.Save();

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Someting went wrong with deleting" });

            }

            return Json(new { success = true, message = "All ok with deleting" });

        }
    }
}

    
