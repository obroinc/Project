using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClubWestRFC.DataAccess.Data.Repository
{
    public class OrderDetailsRepository : Respository<OrderDetails>, IOrderDetailsRepository
    {

        //for database
        private readonly ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(OrderDetails orderDetails)
        {
            var orderDetailsFromDb = _db.OrderDetails.FirstOrDefault(m => m.Id == orderDetails.Id);
            _db.OrderDetails.Update(orderDetailsFromDb);



            //saving any changes
            _db.SaveChanges();
        }
    }
    
}
