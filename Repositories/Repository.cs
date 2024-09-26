using System.Linq.Expressions;
using BilliardManagement.Data;
using Microsoft.EntityFrameworkCore;
namespace BilliardManagement.Repositories
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        T? GetById(int id);

        /// <summary>
        /// Asynchronously gets an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
        Task<T?> GetByIdAsync(int id);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>An enumerable of all entities.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Asynchronously gets all entities.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of all entities.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Finds an entity based on a predicate and includes related entities if specified.
        /// </summary>
        /// <param name="predicate">The expression to filter entities.</param>
        /// <param name="queryOperation">Optional. A function to apply additional operations on the query.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        T? Find(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? queryOperation = null);

        /// <summary>
        /// Asynchronously finds an entity based on a predicate and includes related entities if specified.
        /// </summary>
        /// <param name="predicate">The expression to filter entities.</param>
        /// <param name="queryOperation">Optional. A function to apply additional operations on the query.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? queryOperation = null);

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        void Add(T entity);

        /// <summary>
        /// Asynchronously adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddAsync(T entity);

        /// <summary>
        /// Adds multiple new entities.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Asynchronously adds multiple new entities.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        void Update(T entity);

        /// <summary>
        /// Removes an existing entity.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        void Remove(T entity);

        /// <summary>
        /// Removes multiple existing entities.
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Checks if an entity exists synchronously based on its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to check.</param>
        /// <returns>True if the entity exists; otherwise, false.</returns>
        bool Exists(int id);

        /// <summary>
        /// Asynchronously checks if an entity exists based on its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to check.</param>
        /// <returns>A task representing the asynchronous operation. The task result is true if the entity exists; otherwise, false.</returns>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// Executes a raw SQL query to fetch entities.
        /// </summary>
        /// <param name="sql">The raw SQL query.</param>
        /// <param name="allowTracking">Specifies whether to enable change tracking.</param>
        /// <returns>An enumerable of entities that match the SQL query.</returns>
        IEnumerable<T> FromSqlQuery(string sql, bool allowTracking = true);

        /// <summary>
        /// Asynchronously executes a raw SQL query to fetch entities.
        /// </summary>
        /// <param name="sql">The raw SQL query.</param>
        /// <param name="allowTracking">Specifies whether to enable change tracking.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of entities that match the SQL query.</returns>
        Task<IEnumerable<T>> FromSqlQueryAsync(string sql, bool allowTracking = true);

        /// <summary>
        /// Retrieves multiple entities based on a predicate.
        /// </summary>
        /// <param name="predicate">The expression used to filter entities.</param>
        /// <param name="queryOperation">Optional. A function to apply additional operations on the query.</param>
        /// <param name="allowTracking">Optional. Indicates whether to enable change tracking.</param>
        /// <returns>An enumerable of entities that match the predicate.</returns>
        IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate,
                               Func<IQueryable<T>, IQueryable<T>>? queryOperation = null,
                               bool allowTracking = true);


        /// <summary>
        /// Asynchronously retrieves multiple entities based on a predicate.
        /// </summary>
        /// <param name="predicate">The expression used to filter entities.</param>
        /// <param name="queryOperation">Optional. A function to apply additional operations on the query.</param>
        /// <param name="allowTracking">Optional. Indicates whether to enable change tracking.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains an enumerable of entities that match the predicate.</returns>
        Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate,
                                          Func<IQueryable<T>, IQueryable<T>>? queryOperation = null,
                                          bool allowTracking = true);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<T> _entities;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _entities = _dbContext.Set<T>();
        }

        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        public T? GetById(int id)
        {
            return _entities.Find(id);
        }

        /// <summary>
        /// Asynchronously gets an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
        public async Task<T?> GetByIdAsync(int id)
        {
            return await _entities.FindAsync(id);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>An enumerable of all entities.</returns>
        public IEnumerable<T> GetAll()
        {
            return _entities.ToList();
        }

        /// <summary>
        /// Asynchronously gets all entities.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of all entities.</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        /// <summary>
        /// Finds an entity based on a predicate and includes related entities.
        /// </summary>
        /// <param name="predicate">The expression to filter entities.</param>
        /// <param name="queryOperation">Optional. A function to apply additional operations on the query.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        public T? Find(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? queryOperation = null)
        {
            var query = _entities.AsQueryable();

            if (queryOperation != null)
            {
                foreach (var includeProperty in queryOperation)
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Asynchronously finds an entity based on a predicate and includes related entities.
        /// </summary>
        /// <param name="predicate">The expression to filter entities.</param>
        /// <param name="queryOperation">Optional. A function to apply additional operations on the query.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity if found; otherwise, null.</returns>
        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? queryOperation = null)
        {
            var query = _entities.AsQueryable();

            if (queryOperation != null)
            {
                foreach (var includeProperty in queryOperation)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// Adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        /// <summary>
        /// Asynchronously adds a new entity.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        /// <summary>
        /// Adds multiple new entities.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        public void AddRange(IEnumerable<T> entities)
        {
            _entities.AddRange(entities);
        }

        /// <summary>
        /// Asynchronously adds multiple new entities.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _entities.AddRangeAsync(entities);
        }

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        /// <summary>
        /// Removes an existing entity.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        /// <summary>
        /// Removes multiple existing entities.
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        public void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }

        /// <summary>
        /// Applies tracking options to the query.
        /// </summary>
        /// <param name="query">The query to apply tracking options to.</param>
        /// <param name="allowTracking">Specifies whether to enable change tracking.</param>
        /// <returns>The query with tracking options applied.</returns>
        private IQueryable<T> ApplyTracking(IQueryable<T> query, bool allowTracking)
        {
            return allowTracking ? query : query.AsNoTracking();
        }

        /// <summary>
        /// Executes a raw SQL query to fetch entities.
        /// </summary>
        /// <param name="sql">The raw SQL query.</param>
        /// <param name="allowTracking">Specifies whether to enable change tracking.</param>
        /// <returns>An enumerable of entities that match the SQL query.</returns>
        public IEnumerable<T> FromSqlQuery(string sql, bool allowTracking = true)
        {
            var query = _entities.FromSqlRaw(sql);
            query = ApplyTracking(query, allowTracking);
            return query.ToList();
        }

        /// <summary>
        /// Asynchronously executes a raw SQL query to fetch entities.
        /// </summary>
        /// <param name="sql">The raw SQL query.</param>
        /// <param name="allowTracking">Specifies whether to enable change tracking.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable of entities that match the SQL query.</returns>
        public async Task<IEnumerable<T>> FromSqlQueryAsync(string sql, bool allowTracking = true)
        {
            var query = _entities.FromSqlRaw(sql);
            query = ApplyTracking(query, allowTracking);
            return await query.ToListAsync();
        }

        /// <summary>
        /// Retrieves multiple entities based on a predicate.
        /// </summary>
        /// <param name="predicate">The expression used to filter entities.</param>
        /// <param name="queryOperation">Optional. A function to apply additional operations on the query.</param>
        /// <param name="allowTracking">Optional. Indicates whether to enable change tracking.</param>
        /// <returns>An enumerable of entities that match the predicate.</returns>
        public IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate,
                                      Func<IQueryable<T>, IQueryable<T>>? queryOperation = null,
                                      bool allowTracking = true)
        {
            var query = _entities.Where(predicate);
            query = ApplyTracking(query, allowTracking);

            if (queryOperation != null)
            {
                query = queryOperation(query);
            }

            return query.ToList();
        }

        /// <summary>
        /// Asynchronously retrieves multiple entities based on a predicate.
        /// </summary>
        /// <param name="predicate">The expression used to filter entities.</param>
        /// <param name="queryOperation">Optional. A function to apply additional operations on the query.</param>
        /// <param name="allowTracking">Optional. Indicates whether to enable change tracking.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains an enumerable of entities that match the predicate.</returns>
        public async Task<IEnumerable<T>> GetManyAsync(Expression<Func<T, bool>> predicate,
                                                       Func<IQueryable<T>, IQueryable<T>>? queryOperation = null,
                                                       bool allowTracking = true)
        {
            var query = _entities.Where(predicate);
            query = ApplyTracking(query, allowTracking);

            if (queryOperation != null)
            {
                query = queryOperation(query);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// Checks if an entity exists synchronously based on its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to check.</param>
        /// <returns>True if the entity exists; otherwise, false.</returns>
        public bool Exists(int id)
        {
            return _entities.Find(id) != null;
        }

        /// <summary>
        /// Asynchronously checks if an entity exists based on its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to check.</param>
        /// <returns>A task representing the asynchronous operation. The task result is true if the entity exists; otherwise, false.</returns>
        public async Task<bool> ExistsAsync(int id)
        {
            var entity = await _entities.FindAsync(id);
            return entity != null;
        }
    }
}