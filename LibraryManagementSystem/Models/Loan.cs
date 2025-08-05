using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Loan: ISoftDelete
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? Returndate { get; set; }
        public LoanStatus Status { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public int BookCopyId { get; set; }
        public BookCopy? BookCopy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
