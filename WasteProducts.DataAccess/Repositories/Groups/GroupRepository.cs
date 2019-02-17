﻿using System;
using System.Collections.Generic;
using System.Linq;
using WasteProducts.DataAccess.Common.Repositories.Groups;
using System.Data.Entity;
using System.Linq.Expressions;
using WasteProducts.DataAccess.Contexts;
using System.Threading.Tasks;

namespace WasteProducts.DataAccess.Repositories.Groups
{
    public class GroupRepository : IGroupRepository
    {
        private readonly WasteContext _context;
        private bool _disposed = false;

        public GroupRepository(WasteContext context)
        {
            _context = context;
        }

        public void Create<T>(T item) where T : class
        {
            _context.Set<T>().Add(item);
        }

        public void Update<T>(T item) where T : class
        {
            _context.Entry(item).State = EntityState.Modified;
        }
 
        public void UpdateAll<T>(IList<T> items) where T : class
        {
            while(items.Count > 0)
            {
                _context.Entry(items[0]).State = EntityState.Modified;
            }
        }
 
        public void Delete<T>(string id) where T : class
        {
            var group = _context.Set<T>().Find(id);
            if (group != null)
                _context.Set<T>().Remove(group);
        }

        public void Delete<T>(T item) where T : class
        {
            _context.Entry(item).State = EntityState.Deleted;
        }

        public void DeleteAll<T>(IList<T> items) where T : class
        {
            while (items.Count > 0)
            {
                _context.Entry(items[0]).State = EntityState.Deleted;
            }
        }

        public Task<T> Get<T>(string id) where T : class
        {
            return Task.FromResult(_context.Set<T>().Find(id));
        }

        public Task<IEnumerable<T>> GetAll<T>() where T : class
        {
            return Task.FromResult((IEnumerable<T>)_context.Set<T>());
        }

        public Task<IEnumerable<T>> Find<T>(Func<T, bool> predicate) where T : class
        {
            return Task.FromResult((IEnumerable<T>)_context.Set<T>().Where(predicate));
        }

        public Task<IEnumerable<T>> GetWithInclude<T>(
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return Task.FromResult((IEnumerable<T>)Include(includeProperties).ToList());
        }

        public Task<IEnumerable<T>> GetWithInclude<T>(Func<T, bool> predicate,
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            var query = Include(includeProperties);
            return Task.FromResult(query.Where(predicate));
        }

        public Task Save()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
                _disposed = true;
            }
        }

        private IQueryable<T> Include<T>(
            params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            IQueryable<T> query = _context.Set<T>();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        ~GroupRepository()
        {
            Dispose();
        }
    }
}
