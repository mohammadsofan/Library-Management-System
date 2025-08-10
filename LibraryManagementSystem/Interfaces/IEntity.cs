namespace LibraryManagementSystem.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime LastUpdatedAt { get; set; }
    }
}
