using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.@abstract
{
    internal interface IRepository<T>
    {
        void Add(T entity);
        void Remove(int id);
        T GetById(int id);
        IEnumerable<T> GetAll();
    }
}
