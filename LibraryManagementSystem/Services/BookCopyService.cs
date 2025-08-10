using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class BookCopyService : Service<BookCopyRequestDto, BookCopyResponseDto, BookCopy>, IBookCopyService
    {
        private readonly IRepository<BookCopy> _repository;

        public BookCopyService(IRepository<BookCopy> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
