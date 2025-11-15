namespace Exercise3.RazorApp.Services;

public interface IUserSession
{
    bool IsAuthenticated { get; }
    int? UserId { get; }
    string? UserName { get; }
    void SignIn(int userId, string userName);
    void SignOut();
}
