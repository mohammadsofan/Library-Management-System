namespace LibraryManagementSystem.Dtos.Requests
{
    public class LoginUserRequestDto
    {
        public string EmailOrUsername { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
