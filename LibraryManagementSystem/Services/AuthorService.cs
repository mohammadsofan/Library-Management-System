using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class AuthorService : Service<AuthorRequestDto, AuthorResponseDto, Author>, IAuthorService
    {
        private readonly IRepository<Author> _repository;

        public AuthorService(IRepository<Author> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
