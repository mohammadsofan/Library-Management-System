using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Review: ISoftDelete, IEntity
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string? Message { get; set; }
        public string ApplicationUserId {  get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
