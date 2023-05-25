namespace RestaurantHomework.Authorization.Dal.Repositories.Interfaces;

public interface ISessionsRepository
{
    Task Add();
    Task Query();
}