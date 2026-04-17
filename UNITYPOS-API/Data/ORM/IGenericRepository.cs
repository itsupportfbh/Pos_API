using System.Linq.Expressions;

namespace UNITYPOS_API.Data.ORM
{
    public interface IGenericRepository<T> where T : class
    {


        IQueryable<T> Table();

        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();

        T? GetById(object id);
        Task<T?> GetByIdAsync(object id);

        T? Find(Expression<Func<T, bool>> predicate);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);

        void Insert(T entity);
        Task InsertAsync(T entity);

        void InsertRange(IEnumerable<T> entities);
        Task InsertRangeAsync(IEnumerable<T> entities);

        void Update(T entity);

        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}
