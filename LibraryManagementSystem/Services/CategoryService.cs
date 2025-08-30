using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Wrappers;

namespace LibraryManagementSystem.Services
{
    public class CategoryService : Service<CategoryRequestDto, CategoryResponseDto, Category>, ICategoryService
    {
        private readonly IRepository<Category> _repository;

        public CategoryService(IRepository<Category> repository) : base(repository)
        {
            _repository = repository;
        }
        public override async Task<ServiceResult<CategoryResponseDto>> CreateAsync(CategoryRequestDto requestDto)
        {
            var isExist = await _repository.AnyAsync(c => c.Name.ToLower() == requestDto.Name.ToLower());
            if (isExist)
            {
                return ServiceResult<CategoryResponseDto>.Fail("Category with the same name already exists.", new List<string> { "Duplicate category name." }, Enums.ServiceResultStatus.Conflict);
            }
            return await base.CreateAsync(requestDto);
        }
        public override async Task<ServiceResult> UpdateAsync(int id, CategoryRequestDto requestDto)
        {
            var isExist = await _repository.AnyAsync(c => c.Name.ToLower() == requestDto.Name.ToLower() && c.Name.ToLower() != requestDto.Name.ToLower());
            if (isExist)
            {
                return ServiceResult.Fail("Category with the same name already exists.", new List<string> { "Duplicate category name." }, Enums.ServiceResultStatus.Conflict);
            }
            return await base.UpdateAsync(id, requestDto);
        }
    }
}
