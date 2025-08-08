using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Dtos.Responses
{
    public class ApplicationUserResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string IdCardNumber { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public int? CityId { get; set; }
    }
}
