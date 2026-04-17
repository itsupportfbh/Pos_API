using System;
using UNITYPOS_API.Data.Context;

namespace UNITYPOS_API.Data.ORM
{
    public interface IUnitOfWork : IDisposable
    {
        POSContext Context { get; }

        void BeginTransaction();
        Task BeginTransactionAsync();

        void Commit();
        Task CommitAsync();

        void Rollback();
        Task RollbackAsync();

        int Save();
        Task<int> SaveAsync();

        IGenericRepository<T> GenericRepository<T>() where T : class;

        void Initialize(string connectionString);
    }
}
