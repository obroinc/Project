using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ClubWestRFC.Models

{
    //Adding additional columns/properties to the memebrs regestry page
    public class ApplicationUser:IdentityUser

    {
        [Display(Name ="Full Name")]
        public string FirstName { get; set; }

        public string LastName { get; set; }


        //wILL not be displayed in the database
        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }


        //Additional Family members 1 of 2

        [Display(Name = "Full Name")]
        public string FirstNameFamily1 { get; set; }

        public string LastNameFamily1 { get; set; }

        [Display(Name ="If a player please enter DOB")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:dd-mm-yy}", ApplyFormatInEditMode =true)]
        public DateTime? DOBFamily1 { get; set; }

        [Display(Name = "What team would they wish to play on?")]
        public string TeamFamily1 { get; set; }

        [NotMapped]
        public string FullNameFamily1 { get { return FirstNameFamily1 + " " + LastNameFamily1; } }

        //Additional Family members 2 of 2

        [Display(Name = "Full Name")]
        public string FirstNameFamily2 { get; set; }

        public string LastNameFamily2 { get; set; }

        [Display(Name = "If a player please enter DOB")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yy}", ApplyFormatInEditMode = true)]
        public DateTime? DOBFamily2 { get; set; }

        [Display(Name = "What team would they wish to play on?")]
        public string TeamFamily2 { get; set; }

        [NotMapped]
        public string FullNameFamily2 { get { return FirstNameFamily2 + " " + LastNameFamily2; } }
    }
}
