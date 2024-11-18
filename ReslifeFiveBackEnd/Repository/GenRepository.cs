using ReslifeFiveBackEnd.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReslifeFiveBackEnd.Repository
{
    public class GenRepository : IGenRepository
    {
        private readonly ApplicationDbContext _context;

        public GenRepository(ApplicationDbContext context)
        {  
            _context = context; 
        }

        public void Add<T>(T entity) where T : class 
        { 
            _context.Set<T>().Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
        }

        public void SaveChanges()
        { 
            _context.SaveChanges();
        }

        public IQueryable<T> Set<T>() where T : class
        {
            return _context.Set<T>();
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            await _context.AddAsync(entity);
        }
    }
}
