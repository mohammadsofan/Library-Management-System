using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Author: ISoftDelete
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set;} = string.Empty;
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
