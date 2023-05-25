using System.Security.Cryptography;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using RestaurantHomework.Authorization.Dal.Entities;
using RestaurantHomework.Authorization.Dal.Repositories.Interfaces;

namespace RestaurantHomework.Authorization.Bll.Commands;

public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<Unit>;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Unit>
{
    private readonly IUsersRepository _usersRepository;
    
    public RegisterUserHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var searchByUsername = await _usersRepository.QueryByUsername(request.Username, cancellationToken);
        var searchByEmail = await _usersRepository.QueryByEmail(request.Email, cancellationToken);
        if (searchByUsername != null || searchByEmail != null)
        {
            throw new ArgumentException("User already exists");
        }
        
        var user = new UserEntity
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = GetPasswordHash(request.Password),
            Role = "customer"
        };
        
        await _usersRepository.Add(user, cancellationToken);
        
        return Unit.Value;
    }

    private static string GetPasswordHash(string password)
    {
        // https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-7.0
        byte[] salt = Enumerable.Range(0, 16).Select(x => (byte)x).ToArray();
        
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 32));

        return hashed;
    }
}