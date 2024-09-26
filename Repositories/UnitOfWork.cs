using BilliardManagement.Data;
using BilliardManagement.Data.Entities;

namespace BilliardManagement.Repositories
{
    /// <summary>
    /// Implements the Unit of Work pattern to manage multiple repository operations.
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        public IRepository<Club> _clubRepository { get; }
        public IRepository<Branch> _branchRepository { get; }
        public IRepository<Table> _tableRepository { get; }
        public IRepository<PriceList> _priceListRepository { get; }

        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="dbContext">The database context to use.</param>
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            // Assign the same DbContext instance to each repository
            _clubRepository = new Repository<Club>(_dbContext);
            _branchRepository = new Repository<Branch>(_dbContext);
            _tableRepository = new Repository<Table>(_dbContext);
            _priceListRepository = new Repository<PriceList>(_dbContext);
        }

        /// <summary>
        /// Commits all changes made in the current transaction.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        /// <summary>
        /// Asynchronously commits all changes made in the current transaction.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
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
                }
                _disposed = true;
            }
        }
    }
}
