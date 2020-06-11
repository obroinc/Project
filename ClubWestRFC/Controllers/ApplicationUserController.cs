using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClubWestRFC.Controllers
{
    //
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : Controller
    {
        private readonly IUnitofWork _unitofwork;

        //need to add in the startup.cs  services.AddScoped<IUnitofWork, UnitofWork>();
        //Need unit of work allows to access the database
        public ApplicationUserController(IUnitofWork unitofwork)
        {
            _unitofwork = unitofwork;

        }

        //Retreiving all of the categories and return back
        [HttpGet]
        public IActionResult Get()
        {
            return Json(new {data = _unitofwork.ApplicationUser.GetAll() });
        }
        //If a member or other role in the club needs to be locked out of account for any reason
        [HttpPost]
        public IActionResult LockUnlock([FromBody]string  Id)
        {
            var objFromDb = _unitofwork.ApplicationUser.GetFirstOrDefault(u => u.Id == Id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Someting went wrong while unlocking/locking" });
            }
            //abilty to lock unlock a/c
            if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else //lock out for 20 years
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(20);
            }

            _unitofwork.Save();


            return Json(new { success = true, message = "OK with unlocking/locking" });
        }

    }
}

