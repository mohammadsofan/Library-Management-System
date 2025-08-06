using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Dtos.Responses
{
    public class LoanResponseDto
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? Returndate { get; set; }
        public LoanStatus Status { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public int BookCopyId { get; set; }
    }
}
