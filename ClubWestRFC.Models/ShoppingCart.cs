using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClubWestRFC.Models
{
    public class ShoppingCart

    {
        //Set count to always start at 1
        public ShoppingCart()
        {
        Count=1;    
        }
        public int Id { get; set; }


        //Binding the Id with foreign key references, when not mapped
        //so this db will only have the 4 items
        [NotMapped]
        public int MemberpriceId { get; set; }

        [ForeignKey("MemberpriceId")]
        public virtual Memberprice Memberprice{ get; set; }


        [NotMapped]
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }


        [Range(1,10, ErrorMessage ="Please pick between 1 and 10")]
        public int Count { get; set; }

    }
}
