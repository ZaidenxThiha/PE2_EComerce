using Microsoft.AspNetCore.Http;

namespace Exercise3.RazorApp.Services;

public class UserSession(IHttpContextAccessor accessor) : IUserSession
{
    private const string UserIdKey = "UserId";
    private const string UserNameKey = "UserName";

    private readonly IHttpContextAccessor _accessor = accessor;

    private ISession Session => _accessor.HttpContext?.Session ?? throw new InvalidOperationException("Session is not available");

    public bool IsAuthenticated => Session.GetInt32(UserIdKey) is not null;

    public int? UserId => Session.GetInt32(UserIdKey);

    public string? UserName => Session.GetString(UserNameKey);

    public void SignIn(int userId, string userName)
    {
        Session.SetInt32(UserIdKey, userId);
        Session.SetString(UserNameKey, userName);
    }

    public void SignOut()
    {
        Session.Remove(UserIdKey);
        Session.Remove(UserNameKey);
    }
}
