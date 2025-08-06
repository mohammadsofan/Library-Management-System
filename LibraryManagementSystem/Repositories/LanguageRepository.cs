using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        private readonly ApplicationDbContext _context;

        public LanguageRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
