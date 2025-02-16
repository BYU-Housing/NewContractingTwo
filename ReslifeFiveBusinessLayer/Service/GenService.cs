﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GenService> _logger;   
        public GenService(IGenRepository repo, ILogger<GenService> logger)
        {
            _repo = repo;
            _logger = logger;
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
        public async Task GetModelInBatchesAsync<T>(List<T> targetList, int batchSize, Func<T, int> keySelector) where T : class
        {

            int offset = 0;
            bool hasMoreData = true;
            targetList.Clear();

            while (hasMoreData)
            {
                var batch = _repo.Set<T>()
                                       .OrderBy(keySelector)
                                       .Skip(offset)
                                       .Take(batchSize)
                                       .ToList();


                if (batch.Count == 0)
                {
                    hasMoreData = false; // No more data to load
                }
                else
                {
                    // Add the batch to the target list
                    targetList.AddRange(batch);
                    //foreach (var item in batch)
                    //{
                    //    targetList.Add(item);
                    //}                   

                    offset += batchSize;

                    // Trigger UI updates
                    await Task.Yield();

                }
            }

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

        public async Task UpdateSinglePropertyAsync<T>(int id, string propertyName, object newValue) where T : class
        {

            try
            {
                var objectToUpdate = await GetByIdAsync<T>(id);
                if (objectToUpdate != null)
                {
                    var propToUpdate = typeof(T).GetProperties().FirstOrDefault(x => x.Name == propertyName);
                   if(propToUpdate != null)
                   {
                        propToUpdate.SetValue(objectToUpdate, newValue);
                        await _repo.SaveChangesAsync();
                   }
                   else
                   {
                        _logger.LogWarning($"Tried to update property of {typeof(T)} object, but no property match was found  (genService.UpdateSinglePropertyAsync())");
                   }
                    
                }
                else
                {
                    throw new InvalidOperationException($"Could not retrieve item to update (genService.UpdateSinglePropertyAsync())");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating property ({ex.Message})");
            }
        }

    }
}
