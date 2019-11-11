using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        void Insert(T entity);
        void Delete(T entity);
        void Update(T entity);

    }
}
