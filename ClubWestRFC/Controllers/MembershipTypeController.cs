using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClubWestRFC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipTypeController : Controller
    {

        private readonly IUnitofWork _unitofwork;

        //need to add in the startup.cs  services.AddScoped<IUnitofWork, UnitofWork>();
        //Need unit of work allows to access the database

        public MembershipTypeController(IUnitofWork unitofwork)
        {
            _unitofwork = unitofwork;

        }

        //Retreiving all of the categories and return back
        [HttpGet]
        public IActionResult Get()
        {
             return Json(new { data = _unitofwork.MembershipType.GetAll() });
        }
        //for Delete
        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
             var objFromDb = _unitofwork.MembershipType.GetFirstOrDefault(u => u.Id == Id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Someting went wrong with deleting" });
            }
             _unitofwork.MembershipType.Remove(objFromDb);
            _unitofwork.Save();

            return Json(new { success = true, message = "All ok with deleting" });

        }
    }
}

    
