using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Dtos.Responses
{
    public class BookCopyResponseDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BarCode { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
    }
}
