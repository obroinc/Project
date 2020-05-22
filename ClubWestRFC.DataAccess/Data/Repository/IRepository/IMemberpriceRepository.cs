using ClubWestRFC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClubWestRFC.DataAccess.Data.Repository.IRepository
{
    public interface IMemberpriceRepository:IRepository<Memberprice>
    {
        void Update(Memberprice memberprice);

    }
}
