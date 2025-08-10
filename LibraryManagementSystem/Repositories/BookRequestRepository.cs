using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class BookRequestRepository : Repository<BookRequest>, IBookRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRequestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
