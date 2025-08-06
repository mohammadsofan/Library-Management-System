using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class SystemSetting: IEntity,ISoftDelete
    {
        public int Id { get; set; }

        public string Key { get; set; } = string.Empty; 

        public string Value { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
}
}
