namespace LibraryManagementSystem.Dtos.Requests
{
    public class ReviewRequestDto
    {
        public int Rate { get; set; }
        public string? Message { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public int BookId { get; set; }
    }
}
