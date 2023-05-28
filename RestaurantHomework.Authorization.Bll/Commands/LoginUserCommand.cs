using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RestaurantHomework.Authorization.Bll.Exceptions;
using RestaurantHomework.Authorization.Bll.Models;
using RestaurantHomework.Authorization.Bll.Options;
using RestaurantHomework.Authorization.Bll.Services.Interfaces;
using RestaurantHomework.Authorization.Bll.Utils;
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
            throw new IncorrectDataException("Введены неверные данные: пользователя либо не существует," +
                                             " либо пароль указан неверно");
        }

        if (user.PasswordHash != PasswordHasher.HashPassword(request.Password))
        {
            throw new IncorrectDataException("Введены неверные данные: пользователя либо не существует," +
                                             " либо пароль указан неверно");
        }

        var model = new CreateSessionModel(
            new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Username),
                new(ClaimTypes.Role,user.Role)
            },
            user.Id);

        var token = await _sessionService.CreateSession(model, cancellationToken);

        return new LoginUserResult(token);
    }
}