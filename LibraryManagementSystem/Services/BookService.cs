using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Wrappers;

namespace LibraryManagementSystem.Services
{
    public class BookService : Service<BookRequestDto, BookResponseDto, Book>, IBookService
    {
        private readonly IRepository<Book> _repository;

        public BookService(IRepository<Book> repository) : base(repository)
        {
            _repository = repository;
        }
        public override async Task<ServiceResult<BookResponseDto>> CreateAsync(BookRequestDto dto)
        {
            // image file saving...
            return await base.CreateAsync(dto);
        }
    }
}
