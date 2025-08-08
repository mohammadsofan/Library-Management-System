namespace LibraryManagementSystem.Dtos.Requests
{
    public class ChangeUserPasswordRequestDto
    {
        public string UserId { get; set; } = string.Empty;
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty; 
    }
}
