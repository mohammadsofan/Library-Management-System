using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class SystemSettingService : Service<SystemSettingRequestDto, SystemSettingResponseDto, SystemSetting>, ISystemSettingService
    {
        private readonly IRepository<SystemSetting> _repository;

        public SystemSettingService(IRepository<SystemSetting> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
