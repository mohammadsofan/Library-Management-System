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

        public DBInitializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            if (!_context.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole() { Name = ApplicationRoles.Admin });
                await _roleManager.CreateAsync(new IdentityRole() { Name = ApplicationRoles.Librarian });
                await _roleManager.CreateAsync(new IdentityRole() { Name = ApplicationRoles.Member });
                var applicationUser = new ApplicationUser()
                {
                    IdCardNumber = "0000000000",
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
            if (!_context.Cities.Any())
            {
                _context.Cities.AddRange(new List<City>()
                {
                    new City(){ Name="New York"},
                    new City(){ Name="Los Angeles"},
                    new City(){ Name="Chicago"},
                    new City(){ Name="Houston"},
                    new City(){ Name="Phoenix"},
                    new City(){ Name="Philadelphia"},
                    new City(){ Name="San Antonio"},
                    new City(){ Name="San Diego"},
                    new City(){ Name="Dallas"},
                    new City(){ Name="San Jose"}
                });
                await _context.SaveChangesAsync();
            }
            if (!_context.SystemSettings.Any())
            {
                _context.SystemSettings.AddRange(new List<SystemSetting>()
                {
                    new SystemSetting(){ Key="MaxBooksPerMember",Value="5"},
                    new SystemSetting(){ Key="LoanDurationDays",Value="14"},
                    new SystemSetting(){ Key="FinePerDay",Value="1"},
                });
                await _context.SaveChangesAsync();
            }
            if (!_context.Languages.Any())
            {
                _context.Languages.AddRange(new List<Language>()
                {
                    new Language(){ Name="English"},
                    new Language(){ Name="Spanish"},
                    new Language(){ Name="French"},
                    new Language(){ Name="German"},
                    new Language(){ Name="Chinese"},
                    new Language(){ Name="Japanese"},
                    new Language(){ Name="Russian"},
                    new Language(){ Name="Arabic"},
                    new Language(){ Name="Portuguese"},
                    new Language(){ Name="Hindi"}
                });
                await _context.SaveChangesAsync();
            }
            if (!_context.Categories.Any())
            {
                _context.Categories.AddRange(new List<Category>()
                {
                    new Category(){ Name="Fiction"},
                    new Category(){ Name="Non-Fiction"},
                    new Category(){ Name="Science Fiction"},
                    new Category(){ Name="Fantasy"},
                    new Category(){ Name="Mystery"},
                    new Category(){ Name="Biography"},
                    new Category(){ Name="History"},
                    new Category(){ Name="Romance"},
                    new Category(){ Name="Horror"},
                    new Category(){ Name="Self-Help"}
                });
                await _context.SaveChangesAsync();
            }
            if (!_context.Authors.Any())
            {
                _context.Authors.AddRange(new List<Author>()
                {
                    new Author(){ FirstName="George",LastName="Orwell"},
                    new Author(){ FirstName="J.K.",LastName="Rowling"},
                    new Author(){ FirstName="J.R.R.",LastName="Tolkien"},
                    new Author(){ FirstName="Agatha",LastName="Christie"},
                    new Author(){ FirstName="Stephen",LastName="King"},
                    new Author(){ FirstName="Isaac",LastName="Asimov"},
                    new Author(){ FirstName="Mark",LastName="Twain"},
                    new Author(){ FirstName="Ernest",LastName="Hemingway"},
                    new Author(){ FirstName="Jane",LastName="Austen"},
                    new Author(){ FirstName="Charles",LastName="Dickens"}
                });
                await _context.SaveChangesAsync();
            }
            if (!_context.Publishers.Any())
            {
                _context.Publishers.AddRange(new List<Publisher>()
                {
                    new Publisher(){ Name="Penguin Random House"},
                    new Publisher(){ Name="HarperCollins"},
                    new Publisher(){ Name="Simon & Schuster"},
                    new Publisher(){ Name="Hachette Book Group"},
                    new Publisher(){ Name="Macmillan Publishers"},
                    new Publisher(){ Name="Scholastic"},
                    new Publisher(){ Name="Bloomsbury"},
                    new Publisher(){ Name="Oxford University Press"},
                    new Publisher(){ Name="Cambridge University Press"},
                    new Publisher(){ Name="Wiley"}
                });
                await _context.SaveChangesAsync();
            }
            if (!_context.Books.Any())
            {
                var author1 = await _context.Authors.FirstAsync(a => a.LastName == "Orwell");
                var author2 = await _context.Authors.FirstAsync(a => a.LastName == "Rowling");
                var category1 = await _context.Categories.FirstAsync(c => c.Name == "Fiction");
                var category2 = await _context.Categories.FirstAsync(c => c.Name == "Fantasy");
                var language = await _context.Languages.FirstAsync(l => l.Name == "English");
                var publisher = await _context.Publishers.FirstAsync(p => p.Name == "Penguin Random House");
                _context.Books.AddRange(new List<Book>()
                {
                    new Book()
                    {
                        Title="1984",
                        Subtitle="A Novel",
                        ISBN="9780451524935",
                        Description = "A dystopian social science fiction novel and cautionary tale about the dangers of totalitarianism.",
                        Pages=328,
                        PublicationYear=1999,
                        AuthorId=author1.Id,
                        CategoryId=category1.Id,
                        LanguageId=language.Id,
                        PublisherId=publisher.Id,
                    },
                    new Book()
                    {
                        Title="Harry Potter and the Sorcerer's Stone",
                        Subtitle="Harry Potter and the Philosopher's Stone",
                        ISBN="9780590353427",
                        Description = "A young wizard's journey begins.",
                        Pages=309,
                        PublicationYear=1997,
                        AuthorId=author2.Id,
                        CategoryId=category2.Id,
                        LanguageId=language.Id,
                        PublisherId=publisher.Id,
                    }
                });
                await _context.SaveChangesAsync();
                if (_context.Books.Any())
                {
                    var book1 = await _context.Books.FirstAsync(b => b.Title == "1984");
                    var book2 = await _context.Books.FirstAsync(b => b.Title.Contains("Harry Potter"));
                    _context.BookCopies.AddRange(new List<BookCopy>()
                    {
                        new BookCopy(){ BookId = book1.Id, IsAvailable = true, BarCode = "BC0001"},
                        new BookCopy(){ BookId = book1.Id, IsAvailable = true, BarCode = "BC0002"},
                        new BookCopy(){ BookId = book1.Id, IsAvailable = true, BarCode = "BC0003"},
                        new BookCopy(){ BookId = book2.Id, IsAvailable = true, BarCode = "BC0004"},
                        new BookCopy(){ BookId = book2.Id, IsAvailable = true, BarCode = "BC0005"},
                        new BookCopy(){ BookId = book2.Id, IsAvailable = true, BarCode = "BC0006"},
                    });
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
