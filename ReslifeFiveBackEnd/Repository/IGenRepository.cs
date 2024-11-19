using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReslifeFiveBackEnd.Repository
{
    public interface IGenRepository
    {
        void Add<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void SaveChanges();
        IQueryable<T> Set<T>() where T : class;
        public Task AddAsync<T>(T entity) where T : class;
        Task SaveChangesAsync();

    }
}
