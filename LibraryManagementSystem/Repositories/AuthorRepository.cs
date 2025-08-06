using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context) : base(context)
        {
           _context = context;
        }
    }
}
