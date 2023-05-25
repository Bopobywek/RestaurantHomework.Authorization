using Dapper;
using Microsoft.Extensions.Options;
using RestaurantHomework.Authorization.Dal.Entities;
using RestaurantHomework.Authorization.Dal.Options;
using RestaurantHomework.Authorization.Dal.Repositories.Interfaces;

namespace RestaurantHomework.Authorization.Dal.Repositories;

class SessionsRepository : BaseRepository, ISessionsRepository
{
    public SessionsRepository(IOptions<DalOptions> dalSettings) : base(dalSettings.Value)
    {
    }
    
    public async Task Add(SessionEntity sessionEntity, CancellationToken cancellationToken)
    {
        const string sqlInsert = @"insert into sessions (user_id, session_token, expires_at)
values (@UserId, @SessionToken, @ExpiresAt)";

        var sqlInsertParams = new
        {
            sessionEntity.UserId,
            sessionEntity.SessionToken,
            sessionEntity.ExpiresAt
        };
        
        await using var connection = await GetAndOpenConnection();
        await connection.ExecuteAsync(
            new CommandDefinition(
                sqlInsert,
                sqlInsertParams,
                cancellationToken: cancellationToken));
    }

    public async Task<SessionEntity?> Query(string token, CancellationToken cancellationToken)
    {
        const string sqlQuery = @"select id, user_id, session_token, expires_at from sessions where session_token = @Token";

        var sqlQueryParams = new
        {
            Token = token
        };
        
        await using var connection = await GetAndOpenConnection();
        var result = await connection.QueryAsync<SessionEntity>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams,
                cancellationToken: cancellationToken));

        return result.FirstOrDefault();
    }
}