namespace LibraryManagementSystem.Interfaces.IServices
{
    public interface ITokenService
    {
        string GetToken(string id,string email,string role);
    }
}
