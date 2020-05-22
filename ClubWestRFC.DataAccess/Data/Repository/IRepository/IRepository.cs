using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ClubWestRFC.DataAccess.Data.Repository.IRepository
{
    //Class T is genric so that any object can call this e.g. category, memerTypes etc,..
    public interface IRepository<T> where T:class
    {
        T Get(int id);

        //Get all to get a list of category and be able to filter the request using LINQ
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
                Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
                string includeProperties = null
                );

        //Returning only one object from a filter criteria
        T GetFirstOrDefault(
             Expression<Func<T, bool>> filter = null,
                            string includeProperties = null
            );
        //Functions to add or remove an entity from our database

        //adding an object to database
        void Add(T entity);
        //Remove entity if using its id
        void Remove(int id);
        //Remove entity if usser passes the entity
        void Remove(T entity);
    }
}
