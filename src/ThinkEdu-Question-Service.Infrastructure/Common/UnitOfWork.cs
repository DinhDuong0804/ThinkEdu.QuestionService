using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using ThinkEdu_Question_Service.Application.Common;
using ThinkEdu_Question_Service.Infrastructure.Persistence;
using EFCore.BulkExtensions;

namespace ThinkEdu_Question_Service.Infrastructure.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ThinkEduContext _context;

        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(
            ThinkEduContext context
        )
        {
            _context = context;
            _repositories ??= new Dictionary<Type, object>();

        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new BaseRepository<TEntity>(_context);
            }

            return (BaseRepository<TEntity>)_repositories[type];
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BulkSaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.BulkSaveChangesAsync(cancellationToken: cancellationToken);
        }

        public void LazyLoadingEnabledFalse()
        {
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _context.ChangeTracker.LazyLoadingEnabled = false;
        }

        public async Task RollbackAsync()
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            await transaction.RollbackAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}