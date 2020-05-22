using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using ClubWestRFC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
//

namespace ClubWestRFC.DataAccess.Data.Repository
{
    public class UnitofWork:IUnitofWork
    {
        private readonly ApplicationDbContext _db;


        public UnitofWork(ApplicationDbContext db)
        {

            _db = db;

            Category = new CategoryRespository(_db);
            MembershipType = new MembershipTypeRepository(_db);
            Memberprice = new MemberpriceRepository(_db);



        }
        //setting inside constructor, private so it is not set out of this
        public ICategoryRepository Category { get; private set; }

        public IMembershipTypeRepository MembershipType { get; private set; }

        public IMemberpriceRepository Memberprice { get; set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
