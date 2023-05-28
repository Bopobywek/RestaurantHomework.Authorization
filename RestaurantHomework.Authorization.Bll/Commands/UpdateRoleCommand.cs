using System.Security.Cryptography;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using RestaurantHomework.Authorization.Bll.Exceptions;
using RestaurantHomework.Authorization.Bll.Utils;
using RestaurantHomework.Authorization.Dal.Entities;
using RestaurantHomework.Authorization.Dal.Repositories.Interfaces;

namespace RestaurantHomework.Authorization.Bll.Commands;

public record UpdateRoleCommand(string Email, string Role) : IRequest<Unit>;

public class UpdateRoleCommandHandle : IRequestHandler<UpdateRoleCommand, Unit>
{
    private readonly IUsersRepository _usersRepository;
    
    public UpdateRoleCommandHandle(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var searchByEmail = await _usersRepository.QueryByEmail(request.Email, cancellationToken);
        if (searchByEmail is null)
        {
            throw new IncorrectDataException("Не удалось найти пользователя с заданным email");
        }

        var user = new UserEntity
        {
            Id = searchByEmail.Id,
            Username = searchByEmail.Username,
            Email = searchByEmail.Email,
            PasswordHash = searchByEmail.PasswordHash,
            Role = request.Role
        };
        
        await _usersRepository.Update(user, cancellationToken);
        
        return Unit.Value;
    }

    
}