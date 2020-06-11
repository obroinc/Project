using ClubWestRFC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClubWestRFC.DataAccess.Data.Repository.IRepository
{
    public interface IShoppingCartRepository:IRepository<ShoppingCart>
    {
        //to increment and decremant the cout value


        int IncrementCount(ShoppingCart shoppingCart, int count);


        int DecrementCount(ShoppingCart shoppingCart, int count);
    }
}
