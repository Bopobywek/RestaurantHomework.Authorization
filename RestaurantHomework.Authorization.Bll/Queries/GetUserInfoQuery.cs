using MediatR;
using RestaurantHomework.Authorization.Bll.Exceptions;
using RestaurantHomework.Authorization.Bll.Models;
using RestaurantHomework.Authorization.Dal.Repositories.Interfaces;

namespace RestaurantHomework.Authorization.Bll.Queries;

public record GetUserInfoQuery(string Token) : IRequest<GetUserInfoQueryResult>;

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, GetUserInfoQueryResult>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ISessionsRepository _sessionsRepository;

    public GetUserInfoQueryHandler(IUsersRepository usersRepository, ISessionsRepository sessionsRepository)
    {
        _usersRepository = usersRepository;
        _sessionsRepository = sessionsRepository;
    }

    public async Task<GetUserInfoQueryResult> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var session = await _sessionsRepository.Query(request.Token, cancellationToken);
        if (session is null)
        {
            throw new InvalidTokenException("Передан несуществующий токен");
        }

        var user = await _usersRepository.QueryById(session.UserId, cancellationToken);
        if (user is null)
        {
            throw new IncorrectDataException("По данному токену не удалось найти пользователя");
        }

        return new GetUserInfoQueryResult(user.Username, user.Email, user.Role, user.CreatedAt, user.UpdatedAt);
    }
}
