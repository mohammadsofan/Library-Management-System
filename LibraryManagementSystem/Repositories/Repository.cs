using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces.IRepositrories;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Interfaces;
namespace LibraryManagementSystem.Repositories
{
    public class Repository<T> : IRepository<T> where T : class,IEntity,ISoftDelete
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context) {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public virtual async Task<T> CreateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"Entity cannot be null.");
            }
            try
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.LastUpdatedAt = DateTime.UtcNow;
                await _dbSet.AddAsync(entity);
                await SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    return false;
                }
                entity.IsDeleted = true;
                entity.DeletedAt = DateTime.UtcNow;
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public virtual async Task<ICollection<T>> GetAllByFilterAsync(Expression<Func<T, bool>>? filter = null)
        {
            try
            {
                IQueryable<T> query = _dbSet;
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                var entities = await query.ToListAsync();
                return entities;
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }

        public virtual async Task<T?> GetOneByFilterAsync(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "Filter expression cannot be null.");
            }
            try
            {
                var entity = await _dbSet.FirstOrDefaultAsync(filter);
                return entity;
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }

        public virtual async Task<bool> UpdateAsync(int id, T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }
            try
            {
                var existedEntity = await _dbSet.FindAsync(id);
                if (existedEntity == null)
                {
                    return false;
                }
                entity.LastUpdatedAt = DateTime.UtcNow;
                entity.Id = existedEntity.Id;
                _context.Entry(existedEntity).CurrentValues.SetValues(entity);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter), "Filter expression cannot be null.");
            }
            try
            {
                return await _dbSet.AnyAsync(filter);
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
        public virtual async Task SaveChangesAsync()
        {
            try { 
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
        }
    }
}
