using ClubWestRFC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

//Adding Update functions

namespace ClubWestRFC.DataAccess.Data.Repository.IRepository
{
    //To update any changes
    public interface ICategoryRepository:IRepository<Category>
    {
        // to get a drop down list

        IEnumerable<SelectListItem> GetCategoryListForDropdown();

        //Update by passing the category

        void Update(Category category);

    }
}
