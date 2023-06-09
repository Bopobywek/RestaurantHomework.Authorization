﻿using Dapper;
using Microsoft.Extensions.Options;
using RestaurantHomework.Authorization.Dal.Entities;
using RestaurantHomework.Authorization.Dal.Options;
using RestaurantHomework.Authorization.Dal.Repositories.Interfaces;

namespace RestaurantHomework.Authorization.Dal.Repositories;

public class UsersRepository : BaseRepository, IUsersRepository
{
    public UsersRepository(IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    {
    }
    
    public async Task Add(UserEntity userEntity, CancellationToken token)
    {
        const string sqlInsert = @"insert into users (username, email, password_hash, role)
values (@Username, @Email, @PasswordHash, @Role)";
        
        var sqlInsertParams = new
        {
            userEntity.Username,
            userEntity.Email,
            userEntity.PasswordHash,
            userEntity.Role
        };

        await using var connection = await GetAndOpenConnection();
        await connection.ExecuteAsync(
            new CommandDefinition(
                sqlInsert,
                sqlInsertParams,
                cancellationToken: token));
    }

    public async Task Update(UserEntity userEntity, CancellationToken cancellationToken)
    {
        const string sqlUpdate = @"update users set role = @Role where id = @Id";
        
        var sqlUpdateParams = new
        {
            userEntity.Role
        };
        
        await using var connection = await GetAndOpenConnection();
        await connection.ExecuteAsync(
            new CommandDefinition(
                sqlUpdate,
                sqlUpdateParams,
                cancellationToken: cancellationToken));
    }

    public async Task<UserEntity?> QueryByEmail(string email, CancellationToken token)
    {
        const string sqlQuery = @"
select id,
       username,
       email,
       password_hash,
       role,
       created_at,
       updated_at
from users
where email = @Email";
        
        var sqlQueryParams = new
        {
            Email = email 
        };
        
        await using var connection = await GetAndOpenConnection();
        var result = await connection.QueryAsync<UserEntity>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));

        return result.FirstOrDefault();
    }

    public async Task<UserEntity?> QueryByUsername(string username, CancellationToken token)
    {
        const string sqlQuery = @"
select id,
       username,
       email,
       password_hash,
       role,
       created_at,
       updated_at
from users
where username = @Username";
        
        var sqlQueryParams = new
        {
            Username = username
        };
        
        await using var connection = await GetAndOpenConnection();
        var result = await connection.QueryAsync<UserEntity>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));

        return result.FirstOrDefault();
    }

    public async Task<UserEntity?> QueryById(int id, CancellationToken token)
    {
        const string sqlQuery = @"
select id,
       username,
       email,
       password_hash,
       role,
       created_at,
       updated_at
from users
where id = @Id";
        
        var sqlQueryParams = new
        {
            Id = id
        };
        
        await using var connection = await GetAndOpenConnection();
        var result = await connection.QueryAsync<UserEntity>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: token));

        return result.FirstOrDefault();
    }
}