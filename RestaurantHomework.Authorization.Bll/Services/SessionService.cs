using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestaurantHomework.Authorization.Bll.Models;
using RestaurantHomework.Authorization.Bll.Options;
using RestaurantHomework.Authorization.Bll.Services.Interfaces;
using RestaurantHomework.Authorization.Dal.Entities;
using RestaurantHomework.Authorization.Dal.Repositories.Interfaces;

namespace RestaurantHomework.Authorization.Bll.Services;

public class SessionService : ISessionService
{
    private readonly ISessionsRepository _sessionsRepository;
    private readonly IOptions<JwtOptions> _jwtOptions;

    public SessionService(ISessionsRepository sessionsRepository, IOptions<JwtOptions> jwtOptions)
    {
        _sessionsRepository = sessionsRepository;
        _jwtOptions = jwtOptions;
    }
    
    public async Task<string> CreateSession(CreateSessionModel model, CancellationToken cancellationToken)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.Value.Secret));

        var issuer = "http://mysite.com";
        var audience = "http://myaudience.com";

        var tokenHandler = new JwtSecurityTokenHandler();
        var expires = DateTime.UtcNow.AddSeconds(_jwtOptions.Value.TokenLifetime);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(model.Claims),
            Expires = expires,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);
        await _sessionsRepository.Add(
            new SessionEntity
            {
                UserId = model.UserId,
                SessionToken = stringToken,
                ExpiresAt = expires
            },
            cancellationToken);
        
        return stringToken;
    }

    public bool ValidateToken()
    {
        throw new NotImplementedException();
    }
}