using BilliardManagement.Data;
using BilliardManagement.Data.Entities;
using Microsoft.EntityFrameworkCore.Storage; // For handling transactions

namespace BilliardManagement.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction? _transaction;
        public IRepository<Club> _clubRepository { get; }
        public IRepository<Branch> _branchRepository { get; }
        public IRepository<Table> _tableRepository { get; }
        public IRepository<PriceList> _priceListRepository { get; }

        private bool _disposed = false;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _clubRepository = new Repository<Club>(_dbContext);
            _branchRepository = new Repository<Branch>(_dbContext);
            _tableRepository = new Repository<Table>(_dbContext);
            _priceListRepository = new Repository<PriceList>(_dbContext);
        }

        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        public void CreateTransaction()
        {
            _transaction = _dbContext.Database.BeginTransaction();
        }

        /// <summary>
        /// Starts a new transaction asynchronously.
        /// </summary>
        public async Task CreateTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        public void Commit()
        {
            try
            {
                SaveChanges();
                _transaction?.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
            }
        }

        /// <summary>
        /// Commits the current transaction asynchronously.
        /// </summary>
        public async Task CommitAsync()
        {
            try
            {
                await SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                }
            }
        }

        /// <summary>
        /// Rolls back the current transaction.
        /// </summary>
        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
        }

        /// <summary>
        /// Rolls back the current transaction asynchronously.
        /// </summary>
        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
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
                    _dbContext.Dispose();
                    _transaction?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
