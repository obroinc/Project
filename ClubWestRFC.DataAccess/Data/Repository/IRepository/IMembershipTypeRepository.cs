using ClubWestRFC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClubWestRFC.DataAccess.Data.Repository.IRepository
{
   public interface IMembershipTypeRepository:IRepository<MembershipType>
    {

        // to get a drop down list

        IEnumerable<SelectListItem> GetMembershipTypeListForDropdown();

        //Update by passing the category

        void Update(MembershipType membershiptype);



    }
}
