using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
