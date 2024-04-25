namespace LabU.Core.Interfaces
{
    public interface IAuthService
    {
        Task<bool> TryAuthUserAsync(string login, string password);

        int GetLoginAttemptsCount(string login, int maxAttemptsCount = 5);

        int AddLoginAttempt(int userId);

        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<bool> ChangePasswordAsync(int userId, string newPassword);
    }
}
