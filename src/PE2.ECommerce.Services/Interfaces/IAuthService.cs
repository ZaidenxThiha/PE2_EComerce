using System.Threading.Tasks;
using PE2.ECommerce.Services.Models;

namespace PE2.ECommerce.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResult> LoginAsync(string userName, string password);
}
