using System.Security.Claims;

namespace RestaurantHomework.Authorization.Bll.Models;

public record CreateSessionModel(Claim[] Claims, int UserId);