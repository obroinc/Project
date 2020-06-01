using System;
using System.Collections.Generic;
using System.Text;

namespace ClubWestRFC.Models.ViewModels
{
    //used inside shoppin cart, To display ordertotals 
    public class OrderDetailsCart
    {
        //if more than one items in the shopping cart
        public List<ShoppingCart> listCart { get; set; }

        //
        public OrderHeader OrderHeader { get; set; }
    }
}
