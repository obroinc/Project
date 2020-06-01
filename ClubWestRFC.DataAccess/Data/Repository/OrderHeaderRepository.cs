using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClubWestRFC.DataAccess.Data.Repository
{
    public class OrderHeaderRepository: Respository<OrderHeader>, IOrderHeaderRepository
    {

        //for database
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }


                            

        public void Update(OrderHeader orderHeader)
        {
            var orderHeaderFromDb = _db.OrderHeader.FirstOrDefault(m => m.Id == orderHeader.Id);
            _db.OrderHeader.Update(orderHeaderFromDb);



            //saving any changes
            _db.SaveChanges();
        }



    }
}
