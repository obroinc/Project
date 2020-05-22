using ClubWestRFC.DataAccess.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


//Methods inside Repository

namespace ClubWestRFC.DataAccess.Data.Repository
{
    //T is a generic class
    public class Respository<T> : IRepository<T> where T : class
    {

        //To access the database we use  the dbContext class
        
        protected readonly DbContext Context;
        internal DbSet<T> dbSet;

        //usine dependency injection
        public Respository(DbContext context)
        {
            Context = context;
            this.dbSet = context.Set<T>();
        }


        //Adding entity to our exiting dbSet
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        //here we have 3 parameters i.e. filters, delete order by include properties        
        
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {

            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            //equal loading check and if more than one properties  that are separted by a comma 

            if (includeProperties != null)
            {
                foreach(var includeProperty in includeProperties.Split(new char[] { ',' }, 
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);


                } 

            }
            //Check query using orderby class and return if not null, convert to a list
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {

            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            //equal loading check and if more than one properties  that are separted by a comma 

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' },
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);


                }

            }
            //only returning the first object that left inside the query
            return query.FirstOrDefault();
        }
        //if id is entered to remove the identity is removed also
        public void Remove(int id)
        {
                T entityToRemove = dbSet.Find(id);
                Remove(entityToRemove);
        }
        //if entity is entered to remove the identity is removed also
        public void Remove(T entity)
        {
                dbSet.Remove(entity);
        }
    }
}
