using RestaurantHomework.Authorization.Dal.Entities;

namespace RestaurantHomework.Authorization.Dal.Repositories.Interfaces;

public interface ISessionsRepository
{
    Task Add(SessionEntity sessionEntity, CancellationToken cancellationToken);
    Task<SessionEntity?> Query(string token, CancellationToken cancellationToken);
}