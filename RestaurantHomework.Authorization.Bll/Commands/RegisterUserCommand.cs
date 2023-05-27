using System.Security.Cryptography;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using RestaurantHomework.Authorization.Bll.Exceptions;
using RestaurantHomework.Authorization.Bll.Utils;
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
            throw new UserAlreadyExistsException();
        }
        
        var user = new UserEntity
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = PasswordHasher.HashPassword(request.Password),
            Role = "customer"
        };
        
        await _usersRepository.Add(user, cancellationToken);
        
        return Unit.Value;
    }

    
}