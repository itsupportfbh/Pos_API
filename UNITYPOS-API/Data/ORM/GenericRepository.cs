using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using UNITYPOS_API.Data.Context;

namespace UNITYPOS_API.Data.ORM
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly POSContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(IUnitOfWork uow)
        {
            if (uow == null)
                throw new ArgumentNullException(nameof(uow));

            _context = uow.Context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> Table()
        {
            return _dbSet.AsQueryable();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public T? GetById(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return _dbSet.Find(id);
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return await _dbSet.FindAsync(id);
        }

        public T? Find(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return _dbSet.FirstOrDefault(predicate);
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Add(entity);
        }

        public async Task InsertAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity);
        }

        public void InsertRange(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _dbSet.AddRange(entities);
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _dbSet.RemoveRange(entities);
        }
    }
}
