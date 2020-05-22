using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClubWestRFC.Models.ViewModels
{
     class MemberType
    {


        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Membership Type")]
        public string Name { get; set; }
    }
}
