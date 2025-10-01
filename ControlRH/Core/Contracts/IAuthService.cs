using ControlRH.Areas.Admin.Models;

namespace ControlRH.Core.Contracts;

public interface IAuthService
{
    Task<bool> AutenticarAsync(string login, string senha, HttpContext httpContext, CancellationToken cancellationToken = default);
    Task<string> AutenticarTokenAsync(string login, string senha, CancellationToken cancellationToken = default);

    //Task<Usuario?> Login(string login, string senha, CancellationToken cancellationToken = default);

    //Task Logout(CancellationToken cancellationToken = default);
}
