namespace LibraryManagementSystem.Models
{
    public class PasswordResetCode
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public bool IsUsed { get; set; } = false;
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser? ApplicationUser { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
