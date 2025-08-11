using LibraryManagementSystem.Wrappers;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Interfaces.IServices
{
    public interface IService<TRequest, TResponse,TEntity>
    {
        Task<ServiceResult<TResponse>> CreateAsync(TRequest model);
        Task<ServiceResult> UpdateAsync(int id, TRequest model);
        Task<ServiceResult> DeleteAsync(int id);
        Task<ServiceResult<TResponse>> GetOneByFilterAsync(Expression<Func<TEntity, bool>> filter);
        Task<ServiceResult<ICollection<TResponse>>> GetAllByFilterAsync(Expression<Func<TEntity, bool>>? filter=null);
    }
}
