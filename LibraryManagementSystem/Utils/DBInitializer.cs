using LibraryManagementSystem.Constants;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces;
using LibraryManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Utils
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitializer(ApplicationDbContext context,UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task Initialize()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex){
                Console.WriteLine(ex);
            }
            if (!_context.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = ApplicationRoles.Admin });
                await _roleManager.CreateAsync(new IdentityRole() { Name = ApplicationRoles.Librarian });
                await _roleManager.CreateAsync(new IdentityRole() { Name = ApplicationRoles.Member });
                var applicationUser = new ApplicationUser()
                {
                    IdCardNumber="0000000000",
                    UserName = "Admin",
                    FirstName = "Admin",
                    LastName = "Admin",
                    Gender = Enums.Gender.Male,
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                };
                var result = await _userManager.CreateAsync(applicationUser, "Admin@123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(applicationUser, ApplicationRoles.Admin);
                }
                else
                {
                    foreach (var e in result.Errors)
                    {
                        Console.WriteLine(e.Description);

                    }
                }
            }
        }
    }
}
