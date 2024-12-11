using Microsoft.EntityFrameworkCore;
using ReslifeFiveBackEnd.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReslifeFiveBusinessLayer.Service
{
    public class GenService : IGenService
    {
        private readonly IGenRepository _repo;
        public GenService(IGenRepository repo)
        {
            _repo = repo;
        }

        public IQueryable<T> GetModel<T>() where T : class
        {
            return _repo.Set<T>();
        }
        public IQueryable<T> GetModel<T>(params Expression<Func<T, object>>[] includes) where T : class
        {
            var query = _repo.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public async Task<T> GetByIdAsync<T>(int id) where T : class
        {
            //retrieves the ID property from the class that you pass in
            var idProperty = typeof(T).GetProperty("Id");

            if (idProperty == null)
            {
                throw new InvalidOperationException($"Type {typeof(T).Name} does not have an 'Id' property.");
            }

            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, "Id");
            var idValue = Expression.Constant(id, typeof(int));
            var equality = Expression.Equal(property, idValue);
            var lambda = Expression.Lambda<Func<T, bool>>(equality, parameter);

            var entity = await _repo.Set<T>().FirstOrDefaultAsync(lambda);

            if (entity == null)
            {
                throw new InvalidOperationException($"{typeof(T).Name} with Id {id} not found.");
            }
            return entity;
        }

        public void Upsert<T>(T entity) where T : class
        {
            var idProperty = typeof(T).GetProperty("Id");

            if (idProperty == null)
            {
                throw new InvalidOperationException($"Type {typeof(T).Name} does not have an 'Id' property.");
            }
            // Get the value of the "ID" property
            var idValue = idProperty.GetValue(entity);
            if (idValue is int id && id == 0)
            {
                // If the ID is 0, add the entity
                _repo.Add(entity);
            }
            else
            {
                // If the ID is not 0, update the entity
                _repo.Update(entity);
            }
            _repo.SaveChanges();
        }
        public async Task UpsertAsync<T>(T entity) where T : class
        {
            var idProperty = typeof(T).GetProperty("Id");

            if (idProperty == null)
            {
                throw new InvalidOperationException($"Type {typeof(T).Name} does not have an 'Id' property.");
            }
            // Get the value of the "ID" property
            var idValue = idProperty.GetValue(entity);
            if (idValue is int id && id == 0)
            {
                // If the ID is 0, add the entity
                await _repo.AddAsync(entity);
            }
            else
            {
                // If the ID is not 0, update the entity
                _repo.Update(entity);
            }
            _repo.SaveChanges();
        }

        public void Add<T>(T entity) where T : class => _repo.Add(entity);
        public void Remove<T>(T entity) where T : class => _repo.Remove(entity);
        public void Update<T>(T entity) where T : class => _repo.Update(entity);
        public void SaveChanges() => _repo.SaveChanges();
        public async Task SaveChangesAsync()
        {
            await _repo.SaveChangesAsync();
        }

        public async Task<int> CountAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await _repo.Set<T>().CountAsync(predicate);
        }


    }
}
