using LibraryManagementSystem.Constants;
using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{ApplicationRoles.Admin},{ApplicationRoles.Librarian}")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryRequestDto request)
        {
            var result = await categoryService.CreateAsync(request);
            if (result.Success)
            {
                return CreatedAtRoute(nameof(GetCategoryByIdAsync), new { id = result.Data!.Id }, ApiResponse.Ok(result.Message ?? ""));
            }
            else if (result.Status == Enums.ServiceResultStatus.Conflict)
            {
                return Conflict(ApiResponse.Fail(result.Message ?? "Conflict occurred.", result.Errors));
            }
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.Fail("An error occurred.", null));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryAsync(int id, [FromBody] CategoryRequestDto request)
        {
            var result = await categoryService.UpdateAsync(id, request);
            if (result.Success)
            {
                return NoContent();
            }
            else if (result.Status == Enums.ServiceResultStatus.NotFound)
            {
                return NotFound(ApiResponse.Fail(result.Message ?? "Not found.", result.Errors));
            }
            else if (result.Status == Enums.ServiceResultStatus.Conflict)
            {
                return Conflict(ApiResponse.Fail(result.Message ?? "Conflict occurred.", result.Errors));
            }
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.Fail("An error occurred.", null));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            var result = await categoryService.DeleteAsync(id);
            if (result.Success)
            {
                return NoContent();
            }
            else if (result.Status == Enums.ServiceResultStatus.NotFound)
            {
                return NotFound(ApiResponse.Fail(result.Message ?? "Not found.", result.Errors));
            }
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.Fail("An error occurred.", null));
        }
        [HttpGet("{id}", Name = "GetCategoryByIdAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var result = await categoryService.GetOneByFilterAsync(c => c.Id == id);
            if (result.Success)
            {
                return Ok(ApiResponse<CategoryResponseDto>.Ok(result.Data!, result.Message ?? ""));
            }
            else if (result.Status == Enums.ServiceResultStatus.NotFound)
            {
                return NotFound(ApiResponse.Fail(result.Message ?? "Not found.", result.Errors));
            }
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.Fail("An error occurred.", null));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var result = await categoryService.GetAllByFilterAsync();
            if (result.Success)
            {
                return Ok(ApiResponse<ICollection<CategoryResponseDto>>.Ok(result.Data!, result.Message ?? ""));
            }
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResponse.Fail("An error occurred.", null));
        }
    }
}
