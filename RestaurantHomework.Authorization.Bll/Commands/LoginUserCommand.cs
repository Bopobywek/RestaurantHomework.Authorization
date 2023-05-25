using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestaurantHomework.Authorization.Bll.Models;
using RestaurantHomework.Authorization.Bll.Options;
using RestaurantHomework.Authorization.Bll.Services.Interfaces;
using RestaurantHomework.Authorization.Dal.Entities;
using RestaurantHomework.Authorization.Dal.Repositories.Interfaces;

namespace RestaurantHomework.Authorization.Bll.Commands;

public record LoginUserCommand(string Email, string Password) : IRequest<LoginUserResult>;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, LoginUserResult>
{
    private readonly ISessionService _sessionService;
    private readonly IUsersRepository _usersRepository;

    public LoginUserHandler(ISessionService sessionService, IUsersRepository usersRepository)
    {
        _sessionService = sessionService;
        _usersRepository = usersRepository;
    }

    public async Task<LoginUserResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.QueryByEmail(request.Email, cancellationToken);
        if (user == null)
        {
            throw new ArgumentException();
        }

        if (user.PasswordHash != GetPasswordHash(request.Password))
        {
        }

        var model = new CreateSessionModel(
            new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Username),
            },
            user.Id);

        var token = await _sessionService.CreateSession(model, cancellationToken);

        return new LoginUserResult(token);
    }

    private static string GetPasswordHash(string password)
    {
        // https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-7.0
        byte[] salt = Enumerable.Range(0, 16).Select(x => (byte) x).ToArray();

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32));

        return hashed;
    }
}