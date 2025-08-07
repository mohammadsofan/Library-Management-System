using LibraryManagementSystem.Dtos.Requests;
using LibraryManagementSystem.Dtos.Responses;
using LibraryManagementSystem.Interfaces.IRepositrories;
using LibraryManagementSystem.Interfaces.IServices;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class LoanService : Service<LoanRequestDto, LoanResponseDto, Loan>,ILoanService
    {
        private readonly IRepository<Loan> _repository;

        public LoanService(IRepository<Loan> repository) : base(repository)
        {
            _repository = repository;
        }
    }
}
