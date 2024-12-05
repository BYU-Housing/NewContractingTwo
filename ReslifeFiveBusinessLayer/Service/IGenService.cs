﻿using ReslifeFiveBackEnd.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReslifeFiveBusinessLayer.Service
{
    public interface IGenService
    {
        public IQueryable<T> GetModel<T>() where T : class;
        public IQueryable<T> GetModel<T>(params Expression<Func<T, object>>[] includes) where T : class;

        public Task<T> GetByIdAsync<T>(int id) where T : class;
        void Upsert<T>(T entity) where T : class;
        void Add<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
