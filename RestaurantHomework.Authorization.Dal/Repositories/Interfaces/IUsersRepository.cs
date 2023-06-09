﻿using RestaurantHomework.Authorization.Dal.Entities;

namespace RestaurantHomework.Authorization.Dal.Repositories.Interfaces;

public interface IUsersRepository : IDbRepository
{
    Task Add(UserEntity userEntity, CancellationToken token);
    Task Update(UserEntity userEntity, CancellationToken cancellationToken);
    Task<UserEntity?> QueryByEmail(string email, CancellationToken token);
    Task<UserEntity?> QueryByUsername(string username, CancellationToken token);
    Task<UserEntity?> QueryById(int id, CancellationToken token);
}