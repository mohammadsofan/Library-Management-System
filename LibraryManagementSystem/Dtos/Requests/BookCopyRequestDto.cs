using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Dtos.Requests
{
    public class BookCopyRequestDto
    {
        public int BookId { get; set; }
        public string BarCode { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;
    }
}
