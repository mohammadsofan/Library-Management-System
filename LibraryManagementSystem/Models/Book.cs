using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Book: ISoftDelete, IEntity
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
        public Category? Category { get; set; }
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public int PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();


    }
}
