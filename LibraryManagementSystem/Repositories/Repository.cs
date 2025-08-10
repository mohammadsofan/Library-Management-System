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
            entity.CreatedAt = DateTime.UtcNow;
            entity.LastUpdatedAt = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if(entity == null)
            {
                return false;
            }
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<ICollection<T>> GetAllByFilterAsync(Expression<Func<T, bool>> filter)
        {
            var entities = await _dbSet.Where(filter).ToListAsync();
            return entities;
        }

        public virtual async Task<T?> GetOneByFilterAsync(Expression<Func<T, bool>> filter)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(filter);
            return entity;
        }

        public virtual async Task<bool> UpdateAsync(int id, T entity)
        {
            var existedEntity = await _dbSet.FindAsync(id);
            if (existedEntity == null)
            {
                return false;
            }
            entity.LastUpdatedAt = DateTime.UtcNow;
            entity.Id = existedEntity.Id;
            _context.Entry(existedEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
