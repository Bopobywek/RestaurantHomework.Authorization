namespace RestaurantHomework.Authorization.Bll.Exceptions;

public class InvalidTokenException: Exception
{
    public InvalidTokenException(string message) : base(message)
    {
    }
}