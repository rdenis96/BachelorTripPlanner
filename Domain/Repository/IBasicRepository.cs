using System.Collections.Generic;

namespace Domain.Repository
{
    public interface IBasicRepository<T>
    {
        T Create(T obj);

        T Update(T obj);

        bool Delete(T obj);

        T GetById(int id);

        ICollection<T> GetAll();
    }
}