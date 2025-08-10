using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class FineService : Service<FineRequestDto, FineResponseDto, Fine>, IFineService
    {
        private readonly IRepository<Fine> _repository;

        public FineService(IRepository<Fine> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
