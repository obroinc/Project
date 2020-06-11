using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace ClubWestRFC.DataAccess.Data.Repository
{
    public class ShoppingCartRepository: Respository<ShoppingCart>, IShoppingCartRepository
    {
        //need a database object to be able to acces the database when delaing with functions inside 
        //Icategory repository

        private readonly ApplicationDbContext _db;

        //to retrive this using the follwing construtor

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int DecrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count -= count;
            return shoppingCart.Count;
        }

        public int IncrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count += count;
            return shoppingCart.Count;
        }
    }  
}
