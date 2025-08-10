using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Interfaces.IServices
{
    public interface IAuthorService:IService<AuthorRequestDto,AuthorResponseDto,Author>
    {
    }
}
