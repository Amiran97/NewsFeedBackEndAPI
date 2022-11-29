using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.Core.Base
{
    public interface IRepository<T>
    {
        T Get(string id);
        IEnumerable<T> GetAll();
        void Create(T item);
        void Update(T item);
        void Delete(T item);
    }
}
