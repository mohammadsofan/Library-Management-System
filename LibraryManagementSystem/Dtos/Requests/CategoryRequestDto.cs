using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.Requests
{
    public class CategoryRequestDto
    {
        [Required(ErrorMessage = "Category name is required.")]
        [MinLength(3, ErrorMessage = "Category name length must be at least 3 characters")]
        [MaxLength(50, ErrorMessage = "Category name length must not exceed 50 characters")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required.")]
        [MinLength(3, ErrorMessage = "Description length must be at least 3 characters")]
        [MaxLength(300, ErrorMessage = "Description length must not exceed 300 characters")]
        public string Description { get; set; } = string.Empty;
    }
}
