using LibraryManagementSystem.Data;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories
{
    public class FineRepository : Repository<Fine>, IFineRepository
    {
        private readonly ApplicationDbContext _context;

        public FineRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
