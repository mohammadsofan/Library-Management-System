using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Dtos.Requests
{
    public class FineRequestDto
    {
        public int LoanId { get; set; }
        public decimal? TotalPaid { get; set; }
        public FineStatus FineStatus { get; set; }
    }
}
