using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClubWestRFC.Models.ViewModels
{
    public class MemberType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display (Name="Type of Membership")]
        public string Name { get; set; }
    }
}
