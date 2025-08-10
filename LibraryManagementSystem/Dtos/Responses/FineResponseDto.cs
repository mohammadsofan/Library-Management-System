using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Dtos.Responses
{
    public class FineResponseDto
    {
        public int Id { get; set; }
        public decimal? TotalPaid { get; set; }
        public FineStatus FineStatus { get; set; }
        public int LoanId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
