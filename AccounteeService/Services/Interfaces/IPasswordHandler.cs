namespace AccounteeService.Services.Interfaces
{
    public interface IPasswordHandler
    {
        void CreateHash(string input, out string hash, out string salt);
        bool IsValid(string input, string hash, string salt);
    }
}