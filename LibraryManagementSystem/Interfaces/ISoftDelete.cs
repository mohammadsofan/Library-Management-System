using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
