using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class LanguageService : Service<LanguageRequestDto, LanguageResponseDto, Language>,ILanguageService
    {
        private readonly IRepository<Language> _repository;

        public LanguageService(IRepository<Language> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
