using Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IService<T>
    {
        T Add(T _object);

        void Update(T _object);

        T Delete(int id);

        IEnumerable<T> GetAll();

        void Save();

    }
}
