using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repository.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;
        void SaveChange();
    }
}
