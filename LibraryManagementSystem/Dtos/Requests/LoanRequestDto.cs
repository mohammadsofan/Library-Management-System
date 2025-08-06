using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Dtos.Requests
{
    public class LoanRequestDto
    {
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? Returndate { get; set; }
        public LoanStatus Status { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public int BookCopyId { get; set; }
    }
}
