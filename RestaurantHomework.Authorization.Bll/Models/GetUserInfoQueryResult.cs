namespace RestaurantHomework.Authorization.Bll.Models;

public record GetUserInfoQueryResult(
    string Username,
    string Email,
    string Role,
    DateTime CreatedAt,
    DateTime UpdatedAt);