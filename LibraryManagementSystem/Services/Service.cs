using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Wrappers;
using Mapster;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Services
{
    public class Service<TRequest,TResponse, TEntity> : IService<TRequest,TResponse,TEntity> where TEntity : class where TRequest : class where TResponse : class
    {
        private readonly IRepository<TEntity> _repository;

        public Service(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual async Task<ServiceResult<TResponse>> CreateAsync(TRequest requestDto)
        {
            var mappedEntity = requestDto.Adapt<TEntity>();
            var entity = await _repository.CreateAsync(mappedEntity);
            var responseDto = entity.Adapt<TResponse>();
            return ServiceResult<TResponse>.Ok(responseDto, $"The {typeof(TEntity).Name} record created successfully!");

        }

        public virtual async Task<ServiceResult> DeleteAsync(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
            {
                return ServiceResult.Fail("Delete operation failed",
                    new List<string> { $"{typeof(TEntity).Name} with ID {id} was not found." });
            }
            return ServiceResult.Ok("Record deleted successfully!");

        }

        public virtual async Task<ServiceResult<ICollection<TResponse>>> GetAllByFilterAsync(Expression<Func<TEntity, bool>> filter)
        {
            var entities = await _repository.GetAllByFilterAsync(filter);
            var responseDto = entities.Adapt<ICollection<TResponse>>();
            return ServiceResult<ICollection<TResponse>>.Ok(responseDto, "Data retrived successfully!");
        }

        public virtual async Task<ServiceResult<TResponse>> GetOneByFilterAsync(Expression<Func<TEntity, bool>> filter)
        {
            var entity = await _repository.GetOneByFilterAsync(filter);
            if(entity is null)
            {
                return ServiceResult<TResponse>.Fail($"{typeof(TEntity).Name} not found.",
                    new List<string> { "No matching record found." });
            }
            var responseDto = entity.Adapt<TResponse>();
            return ServiceResult<TResponse>.Ok(responseDto, "Data retrived successfully!");
        }

        public virtual async Task<ServiceResult> UpdateAsync(int id, TRequest requestDto)
        {
            var entity = requestDto.Adapt<TEntity>();
            var result = await _repository.UpdateAsync(id, entity);
            if (!result)
            {
                return ServiceResult.Fail("Failed to update record",
                    new List<string> { $"{typeof(TEntity).Name} with ID {id} was not found." });
            }
            return ServiceResult.Ok("Record updated successfully!");
        }
    }
}
