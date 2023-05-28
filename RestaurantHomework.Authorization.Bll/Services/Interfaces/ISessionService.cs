using System.Security.Claims;
using RestaurantHomework.Authorization.Bll.Models;

namespace RestaurantHomework.Authorization.Bll.Services.Interfaces;

public interface ISessionService
{
    Task<string> CreateSession(CreateSessionModel model, CancellationToken cancellationToken);
}