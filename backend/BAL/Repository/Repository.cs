using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public  void Insert(T entity)
        {
            var entry = _dbSet.Add(entity);
        }

        public  void Delete(T entity)
        {
            var entry = _dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            var entry = _dbSet.Update(entity);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }


    }
}
