using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class City: ISoftDelete
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
