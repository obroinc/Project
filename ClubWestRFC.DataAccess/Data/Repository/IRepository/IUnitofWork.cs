using System;
using System.Collections.Generic;
using System.Text;

namespace ClubWestRFC.DataAccess.Data.Repository.IRepository
{
    //This will represent what happens in a single transaction and will have access to all od your 
    //repositories and a save method that pushes all changes to the database
    public interface IUnitofWork:IDisposable
    {

        ICategoryRepository Category { get; }

        IMembershipTypeRepository MembershipType { get; }

        IMemberpriceRepository Memberprice { get; }
        void Save();

    }
}
