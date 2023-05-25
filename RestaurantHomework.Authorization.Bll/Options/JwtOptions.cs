namespace RestaurantHomework.Authorization.Bll.Options;

public class JwtOptions
{
    public string Secret { get; set; } = string.Empty;
    public int TokenLifetime { get; set; }
}