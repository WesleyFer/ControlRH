using ControlRH.Areas.Admin.Models;
using ControlRH.Core.Contracts;
using ControlRH.Core.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ControlRH.Services;

public class AuthService : IAuthService
{
    private readonly DbContext _dbContext;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _configuration;

    public AuthService(DbContext dbContext, IMemoryCache cache, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _cache = cache;
        _configuration = configuration;
    }

    public async Task<bool> AutenticarAsync(string login, string senha, HttpContext httpContext, CancellationToken cancellationToken = default)
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString();

        if (!ip.StartsWith("192.168.") && ip != "127.0.0.1" && ip != "::1")
            return false;

        var usuario = await ObterPorLoginAsync(login, cancellationToken);

        _cache.TryGetValue($"login_attempts:{login}", out int attempts);

        if (attempts >= 5)
            return false;

        if (usuario == null || !Utils.VerificaSenha(senha, usuario.SenhaHash))
        {
            _cache.Set($"login_attempts:{login}", attempts + 1, TimeSpan.FromMinutes(15));
            return false;
        }

        _cache.Remove($"login_attempts:{login}");

        if (usuario.Colaborador == null || !usuario.Colaborador.Ativo)
            return false;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario?.Colaborador?.Nome ?? string.Empty),
            new Claim("cpf", usuario?.Colaborador?.Cpf ?? string.Empty),
            new Claim("pis", usuario?.Colaborador?.Pis ?? string.Empty),
            new Claim("colaboradorId", usuario?.ColaboradorId.ToString() ?? string.Empty),
            new Claim("carteiraclienteId", usuario?.Colaborador?.CarteiraClienteId.ToString() ?? string.Empty)
        };

        foreach (var grupo in usuario.UsuariosGrupos)
        {
            var permissoes = grupo.Grupo.GruposPermissoes
                .Select(gp => gp.Permissao?.Chave)
                .Where(chave => !string.IsNullOrWhiteSpace(chave))
                .Distinct();

            foreach (var chave in permissoes)
            {
                claims.Add(new Claim(ClaimTypes.Role, chave!));
            }
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return true;
    }

    public async Task<string> AutenticarTokenAsync(string login, string senha, CancellationToken cancellationToken = default)
    {
        var usuario = await ObterPorLoginAsync(login, cancellationToken);

        if (usuario is null || !Utils.VerificaSenha(senha, usuario.SenhaHash))
            return string.Empty;

        if (usuario.Colaborador == null || !usuario.Colaborador.Ativo)
            return string.Empty;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario?.Colaborador?.Nome ?? string.Empty),
            new Claim("cpf", usuario?.Colaborador?.Cpf ?? string.Empty),
            new Claim("pis", usuario?.Colaborador?.Pis ?? string.Empty),
            new Claim("colaboradorId", usuario?.ColaboradorId.ToString() ?? string.Empty),
            new Claim("carteiraclienteId", usuario?.Colaborador?.CarteiraClienteId.ToString() ?? string.Empty)
        };

        foreach (var grupo in usuario.UsuariosGrupos)
        {
            var permissoes = grupo.Grupo.GruposPermissoes
                .Select(gp => gp.Permissao?.Chave)
                .Where(chave => !string.IsNullOrWhiteSpace(chave))
                .Distinct();

            foreach (var chave in permissoes)
            {
                claims.Add(new Claim(ClaimTypes.Role, chave!));
            }
        }

        return GerarToken(claims);
    }

    private string GerarToken(IEnumerable<Claim> claims)
    {
        var chaveSecreta = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
        var credenciais = new SigningCredentials(new SymmetricSecurityKey(chaveSecreta), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credenciais
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private bool ValidarToken(string tokenString)
    {
        var chaveSecreta = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            tokenHandler.ValidateToken(tokenString, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(chaveSecreta),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return true; // Token válido
        }
        catch (SecurityTokenException)
        {
            return false; // Token inválido
        }
    }
    private async Task<Usuario?> ObterPorLoginAsync(string login, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Usuario>()
                        .Include(c => c.UsuariosGrupos)
                            .ThenInclude(c => c.Grupo)
                                .ThenInclude(c => c.GruposPermissoes)
                                    .ThenInclude(c => c.Permissao)
                        .Include(c => c.Colaborador)
                        .FirstOrDefaultAsync(c => c.Login == login, cancellationToken);
    }
}
