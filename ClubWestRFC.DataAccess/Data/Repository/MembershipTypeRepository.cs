using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClubWestRFC.DataAccess.Data.Repository
{
   public class MembershipTypeRepository:Respository<MembershipType>, IMembershipTypeRepository
    {

         //need a database object to be able to access the database when 
        //delaing with functions inside IMembershipType repository

        private readonly ApplicationDbContext _db;

        //to retrive this using the follwing construtor

        public MembershipTypeRepository(ApplicationDbContext db):base (db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetMembershipTypeListForDropdown()
        {
            return _db.MembershipType.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(MembershipType membershiptype)
        {
            var objFromDb = _db.MembershipType.FirstOrDefault(s => s.Id == membershiptype.Id);

            objFromDb.Name = membershiptype.Name;
         

            _db.SaveChanges();

        }
    }
}
