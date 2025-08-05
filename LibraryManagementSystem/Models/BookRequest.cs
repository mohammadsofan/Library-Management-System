using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class BookRequest: ISoftDelete, IEntity
    {
        public int Id { get; set; }
        public string BookName { get; set; } = string.Empty;
        public int LanguageId { get; set; }
        public Language? Language { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
