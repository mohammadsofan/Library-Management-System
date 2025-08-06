using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces.IServices
{
    public interface IFineService:IService<FineRequestDto,FineResponseDto,Fine>
    {
    }
}
