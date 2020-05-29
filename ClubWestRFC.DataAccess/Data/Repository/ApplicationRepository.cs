using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using ClubWestRFC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace ClubWestRFC.DataAccess.Data.Repository
{
    public class ApplicationRespository : Respository<ApplicationUser>, IApplicationRepository
    {
        //need a database object to be able to acces the database when delaing with functions inside 
        //Icategory repository

        private readonly ApplicationDbContext _db;

        //to retrive this using the follwing construtor

        public ApplicationRespository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }  
}
