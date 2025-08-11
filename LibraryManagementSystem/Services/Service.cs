using LibraryManagementSystem.Enums;
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
            try
            {
                var mappedEntity = requestDto.Adapt<TEntity>();
                var entity = await _repository.CreateAsync(mappedEntity);
                var responseDto = entity.Adapt<TResponse>();
                return ServiceResult<TResponse>.Ok(responseDto, $"The {typeof(TEntity).Name} record created successfully!");
            }
            catch (Exception ex)
            {
                return ServiceResult<TResponse>.Fail("Failed to create record.", new List<string> { ex.Message });
            }
        }

        public virtual async Task<ServiceResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _repository.DeleteAsync(id);
                if (!result)
                {
                    return ServiceResult.Fail(
                        $"{typeof(TEntity).Name} not found.",
                        new List<string> { $"{typeof(TEntity).Name} with ID {id} was not found." },
                        ServiceResultStatus.NotFound
                        );
                }
                return ServiceResult.Ok("Record deleted successfully!");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Failed to delete record.", new List<string> { ex.Message });
            }
        }

        public virtual async Task<ServiceResult<ICollection<TResponse>>> GetAllByFilterAsync(Expression<Func<TEntity, bool>>? filter=null)
        {
            try
            {
                var entities = await _repository.GetAllByFilterAsync(filter);
                var responseDto = entities.Adapt<ICollection<TResponse>>();
                return ServiceResult<ICollection<TResponse>>.Ok(responseDto, "Data retrived successfully!");
            }
            catch (Exception ex)
            {
                return ServiceResult<ICollection<TResponse>>.Fail("Failed to retrieve data.", new List<string> { ex.Message });
            }
        }

        public virtual async Task<ServiceResult<TResponse>> GetOneByFilterAsync(Expression<Func<TEntity, bool>> filter)
        {
            if (filter is null)
            {
                return ServiceResult<TResponse>.Fail(
                    "Filter cannot be null.",
                    new List<string> { "Please provide a valid filter expression." },
                    ServiceResultStatus.ValidationError
                    );
            }
            try
            {
                var entity = await _repository.GetOneByFilterAsync(filter);
                if(entity is null)
                {
                    return ServiceResult<TResponse>.Fail(
                        $"{typeof(TEntity).Name} not found.",
                        new List<string> { "No matching record found." },
                        ServiceResultStatus.NotFound
                        );
                }
                var responseDto = entity.Adapt<TResponse>();
                return ServiceResult<TResponse>.Ok(responseDto, "Data retrived successfully!");
            }
            catch (Exception ex)
            {
                return ServiceResult<TResponse>.Fail("Failed to retrieve data.", new List<string> { ex.Message });
            }
        }

        public virtual async Task<ServiceResult> UpdateAsync(int id, TRequest requestDto)
        {
            try
            {
                var entity = requestDto.Adapt<TEntity>();
                var result = await _repository.UpdateAsync(id, entity);
                if (!result)
                {
                    return ServiceResult.Fail(
                        $"{typeof(TEntity).Name} not found.",
                        new List<string> { $"{typeof(TEntity).Name} with ID {id} was not found."},
                        ServiceResultStatus.NotFound
                        );
                }
                return ServiceResult.Ok("Record updated successfully!");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Failed to update record.", new List<string> { ex.Message });
            }
        }
    }
}
