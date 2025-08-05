using LibraryManagementSystem.Enums;
using LibraryManagementSystem.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Models
{
    public class ApplicationUser:IdentityUser,ISoftDelete
    {
        public string IdCardNumber {  get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string Address { get; set; } = string.Empty;
        public int CityId { get; set; }
        public City? City { get; set; }
        public DateTime CreatedAt {get; set; } = DateTime.UtcNow;
        public DateTime LastUpdatedAt { get; set;} = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

    }
}
