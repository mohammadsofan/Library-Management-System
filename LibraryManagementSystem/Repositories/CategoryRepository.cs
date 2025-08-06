using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class CategoryRepository:Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
