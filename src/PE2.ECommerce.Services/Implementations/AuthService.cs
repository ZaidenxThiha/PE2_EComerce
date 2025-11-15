using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PE2.ECommerce.Domain.Data;
using PE2.ECommerce.Services.Interfaces;
using PE2.ECommerce.Services.Models;

namespace PE2.ECommerce.Services.Implementations;

public class AuthService(ECommerceDbContext dbContext) : IAuthService
{
    public async Task<LoginResult> LoginAsync(string userName, string password)
    {
        var hash = CalculateMd5(password);
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        if (user is null)
        {
            return new LoginResult(false, "User not found");
        }

        if (user.Lock)
        {
            return new LoginResult(false, "User is locked");
        }

        if (!string.Equals(user.PasswordHash, hash, System.StringComparison.OrdinalIgnoreCase))
        {
            return new LoginResult(false, "Invalid password");
        }

        return new LoginResult(true, null, user.UserId, user.RoleName);
    }

    private static string CalculateMd5(string value)
    {
        using var md5 = MD5.Create();
        var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
        var builder = new StringBuilder(hashBytes.Length * 2);
        foreach (var b in hashBytes)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }
}
