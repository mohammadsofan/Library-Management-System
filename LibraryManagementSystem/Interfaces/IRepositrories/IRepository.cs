using System.Linq.Expressions;

namespace LibraryManagementSystem.Interfaces.IRepositrories
{
    public interface IRepository<T> where T : class
    {
        Task<T> CreateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(int id, T entity);
        Task<T?> GetOneByFilterAsync(Expression<Func<T, bool>> filter);
        Task<ICollection<T>> GetAllByFilterAsync(Expression<Func<T, bool>>? filter = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        Task SaveChangesAsync();
    }
}
