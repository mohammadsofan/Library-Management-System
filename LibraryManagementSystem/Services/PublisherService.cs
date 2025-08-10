using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class PublisherService : Service<PublisherRequestDto, PublisherResponseDto, Publisher>, IPublisherService
    {
        private readonly IRepository<Publisher> _repository;

        public PublisherService(IRepository<Publisher> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
