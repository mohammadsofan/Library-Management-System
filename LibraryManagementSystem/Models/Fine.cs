using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Fine: ISoftDelete, IEntity
    {
        public int Id { get; set; }
        public decimal TotalFee { get; set; }
        public FineStatus FineStatus { get; set; }
        public int LoanId { get; set; }
        public Loan? Loan { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
