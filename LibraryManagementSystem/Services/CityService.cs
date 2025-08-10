using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class CityService : Service<CityRequestDto, CityResponseDto, City>, ICityService
    {
        private readonly IRepository<City> _repository;

        public CityService(IRepository<City> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
