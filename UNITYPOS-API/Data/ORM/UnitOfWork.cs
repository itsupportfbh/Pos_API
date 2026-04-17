using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System;
using UNITYPOS_API.Data.Context;
using UNITYPOS_API.Entities.DBLog;

namespace UNITYPOS_API.Data.ORM
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        private IDbContextTransaction? _transaction;
        private bool _disposed;
        private Dictionary<string, object>? _repositories;

        public POSContext Context { get; private set; }

        public UnitOfWork(
            POSContext context,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = Context.Database.BeginTransaction();
            }
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await Context.Database.BeginTransactionAsync();
            }
        }

        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            try
            {
                if (_transaction != null)
                {
                    await _transaction.RollbackAsync();
                }
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public int Save()
        {
            try
            {
                var auditEntries = CreateAuditEntries();

                var result = Context.SaveChanges();

                SaveAuditEntries(auditEntries);

                return result;
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database save failed: {innerMessage}", ex);
            }
        }

        public async Task<int> SaveAsync()
        {
            try
            {
                var auditEntries = CreateAuditEntries();

                var result = await Context.SaveChangesAsync();

                await SaveAuditEntriesAsync(auditEntries);

                return result;
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Database save failed: {innerMessage}", ex);
            }
        }

        public IGenericRepository<T> GenericRepository<T>() where T : class
        {
            _repositories ??= new Dictionary<string, object>();

            var typeName = typeof(T).Name;

            if (!_repositories.ContainsKey(typeName))
            {
                var repository = new GenericRepository<T>(this);
                _repositories.Add(typeName, repository);
            }

            return (IGenericRepository<T>)_repositories[typeName];
        }

        public void Initialize(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<POSContext>();

            POSContext.connectionString = connectionString;
            Context = new POSContext(optionsBuilder.Options, _httpContextAccessor, _configuration);

            _repositories?.Clear();
        }

        private List<DBAudit> CreateAuditEntries()
        {
            var auditEntries = new List<DBAudit>();

            Context.ChangeTracker.DetectChanges();

            foreach (var entry in Context.ChangeTracker.Entries())
            {
                if (entry.Entity is DBAudit ||
                    entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                var oldValues = new Dictionary<string, object?>();
                var newValues = new Dictionary<string, object?>();
                var changedColumns = new Dictionary<string, object?>();

                string? action = entry.State switch
                {
                    EntityState.Added => "I",
                    EntityState.Modified => "U",
                    EntityState.Deleted => "D",
                    _ => null
                };

                if (action == null)
                    continue;

                foreach (PropertyEntry property in entry.Properties)
                {
                    var propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                        continue;

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            newValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            oldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (!property.IsModified)
                                continue;

                            oldValues[propertyName] = property.OriginalValue;
                            newValues[propertyName] = property.CurrentValue;
                            changedColumns[propertyName] = property.CurrentValue;
                            break;
                    }
                }

                if (entry.State == EntityState.Modified && changedColumns.Count == 0)
                    continue;

                auditEntries.Add(new DBAudit
                {
                    AuditId = Guid.NewGuid().ToString(),
                    AuditDate = DateTime.Now,
                    TableName = entry.Metadata.GetTableName(),
                    UserName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                    Action = action,
                    OldData = oldValues.Count > 0 ? JsonConvert.SerializeObject(oldValues) : null,
                    NewData = newValues.Count > 0 ? JsonConvert.SerializeObject(newValues) : null,
                    ChangedColumns = changedColumns.Count > 0 ? JsonConvert.SerializeObject(changedColumns) : null
                });
            }

            return auditEntries;
        }

        private void SaveAuditEntries(List<DBAudit> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return;

            try
            {
                Context.Set<DBAudit>().AddRange(auditEntries);
                Context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Audit save failed: {innerMessage}", ex);
            }
        }

        private async Task SaveAuditEntriesAsync(List<DBAudit> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return;

            try
            {
                await Context.Set<DBAudit>().AddRangeAsync(auditEntries);
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                throw new Exception($"Audit save failed: {innerMessage}", ex);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            _transaction?.Dispose();
            Context.Dispose();

            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
