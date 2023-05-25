using RestaurantHomework.Authorization.Dal.Entities;

namespace RestaurantHomework.Authorization.Dal.Repositories.Interfaces;

public interface IUsersRepository
{
    Task Add(UserEntity userEntity, CancellationToken token);
    Task<UserEntity?> QueryByEmail(string email, CancellationToken token);
    Task<UserEntity?> QueryByUsername(string username, CancellationToken token);
    Task<UserEntity?> QueryById(int id, CancellationToken token);
}