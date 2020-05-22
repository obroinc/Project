using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClubWestRFC.DataAccess.Data.Repository
{
    public class MemberpriceRepository : Respository<Memberprice>, IMemberpriceRepository
    {

        //for database
        private readonly ApplicationDbContext _db;
        public MemberpriceRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }


        public void Update(Memberprice memberprice)
        {
            //to find memeberprice from the databae

            var memberpriceFromDb = _db.Memberprice.FirstOrDefault(m => m.Id == memberprice.Id);

            memberpriceFromDb.Name = memberprice.Name;
            memberpriceFromDb.CategoryId = memberprice.CategoryId;
            memberpriceFromDb.Description = memberprice.Description;
            memberpriceFromDb.Price = memberprice.Price;
            memberpriceFromDb.MembershipType = memberprice.MembershipType;

            //For updating an image if it was uploaded

            if (memberprice.image != null)
            {
                memberpriceFromDb.image = memberprice.image;
            }

            //saving any changes
            _db.SaveChanges();

        }
    }
}
