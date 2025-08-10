using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Dtos.Responses
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string? Message { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public int BookId { get; set; }
    }
}
