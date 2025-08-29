namespace LibraryManagementSystem.Dtos.Responses
{
    public class BookResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Subtitle { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Pages { get; set; }
        public int PublicationYear { get; set; }
        public string? ImagePath { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public int LanguageId { get; set; }
    }
}
