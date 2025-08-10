using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Dtos.Responses
{
    public class BookRequestResponseDto
    {
        public int Id { get; set; }
        public string BookName { get; set; } = string.Empty;
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
    }
}
