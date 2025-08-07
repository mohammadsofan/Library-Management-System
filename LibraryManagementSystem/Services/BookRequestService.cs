using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class BookRequestService : Service<BookRequestRequestDto, BookRequestResponseDto, BookRequest>, IBookRequestService
    {
        private readonly IRepository<BookRequest> _repository;

        public BookRequestService(IRepository<BookRequest> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
