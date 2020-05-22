using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace ClubWestRFC.DataAccess.Data.Repository
{
    public class CategoryRespository:Respository<Category>, ICategoryRepository
    {
        //need a database object to be able to acces the database when delaing with functions inside 
        //Icategory repository

        private readonly ApplicationDbContext _db;

        //to retrive this using the follwing construtor

        public CategoryRespository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        //interface being implemented list can be populated from a dropdown
        public IEnumerable<SelectListItem> GetCategoryListForDropdown()
        {
            return _db.Category.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }
        //to write update the database with the object picked inside the category
        public void Update(Category category)
        {

            var objFromDb = _db.Category.FirstOrDefault(s => s.Id == category.Id);

            objFromDb.Name = category.Name;
            objFromDb.DisplayOrder = category.DisplayOrder;

            _db.SaveChanges();


        }
    }
}
