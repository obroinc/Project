using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClubWestRFC.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual OrderHeader OrderHeader { get; set; }


        //to keep the price the same when member  placed an oreder, if membership prices change
        [Required]
        public int MemberpriceId { get; set; }
        [ForeignKey("MemberpriceId")]
        public virtual Memberprice Memberprice{ get; set; }

        public int Count { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

    }
}

