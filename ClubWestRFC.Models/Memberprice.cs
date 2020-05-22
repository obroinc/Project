using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClubWestRFC.Models
{
    public class Memberprice
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        //uploading image to the server and not the datbase
        public string image { get; set; }

        [Range(1,int.MaxValue, ErrorMessage ="Price is greater than 1 Euro")]
        public  double Price{ get; set; }

        //to create a link to the Category table using a foreign key

        [Display(Name ="Category Type")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        //to create a link to the MembershipType table using a foreign key

        [Display(Name = "Membership Type Name")]
        public int MembershipTypeId { get; set; }

        [ForeignKey("MembershipTypeId")]
        public virtual MembershipType MembershipType { get; set; }

    }
}
