using System;
using System.Collections.Generic;
using System.Text;
using ClubWestRFC.DataAccess.Migrations;
using ClubWestRFC.Models;
using ClubWestRFC.Models.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

//Change from Data to DataAccess
//This will be the db contect to access the database when required
namespace ClubWestRFC.DataAccess
{



    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        //to access categories using dbContext application
        public DbSet<Category> Category { get; set; }

        public DbSet<MembershipType>MembershipType { get; set; }

        public DbSet<Memberprice> Memberprice { get; set; }

        //to access Type of Membership using dbContext application
        //  public DbSet<Models.ViewModels.MemberType> MemberType{ get; set; }
        // public DbSet<MemberType1> MemberType1 { get; set; }
    }
}
