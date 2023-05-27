namespace RestaurantHomework.Authorization.Api.Responses;

public record GetInfoResponse(string Username,
    string Email,
    string Role,
    DateTime CreatedAt,
    DateTime UpdatedAt);