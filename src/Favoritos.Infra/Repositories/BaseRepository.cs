using Favoritos.Domain.Entities;
using Favoritos.Domain.Repositories;
using Favoritos.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Favoritos.Infra.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        public DataContext Context { get; set; }
        public DbSet<T> DbSet { get; set; }

        public BaseRepository(DataContext context)
        {
            this.Context = context;
            DbSet = context.Set<T>();
        }



        public void Create(T item)
        {
            DbSet.Add(item);
            Context.SaveChanges();
        }

        //public IEnumerable<T> GetAll()
        //{
        //    var lista = typeof(T).GetProperties().Where(p => p.PropertyType.Namespace.Contains("Collections")).Select(p => p.Name);
        //    var saida = DbSet.AsNoTracking();
        //    foreach (var item in lista)
        //    {
        //        saida = saida.Include(item);
        //    }

        //    return saida.ToList();
        //}

        public IEnumerable<T> GetAll()
        {
            var lista = typeof(T).GetProperties().Where(p => p.PropertyType.Namespace.Contains("Collections")).Select(p => p.Name);
            IQueryable<T> saida = DbSet.Select(i => i);
            foreach (var item in lista)
            {
                saida = saida.Include(item);
            }

            return saida.ToList();
        }

        public T GetById(Guid id)
        {

            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public void Remove(T item)
        {
            Context.Remove(item);//.State;//= EntityState.Modified; ;
            Context.SaveChanges();
        }

        public void Update(T item)
        {
            Context.Entry(item).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
