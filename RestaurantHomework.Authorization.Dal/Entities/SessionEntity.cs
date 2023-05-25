namespace RestaurantHomework.Authorization.Dal.Entities;

public class SessionEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string SessionToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}