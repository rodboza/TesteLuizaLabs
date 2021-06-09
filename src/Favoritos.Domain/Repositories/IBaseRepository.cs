using Favoritos.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Favoritos.Domain.Repositories
{
    public interface IBaseRepository<T> where T : Entity
    {
        void Create(T item);
        void Update(T item);
        void Remove(T item);
        T GetById(Guid id);
        IEnumerable<T> GetAll();

    }
}
