using FluentValidation;
using RestaurantHomework.Authorization.Api.Requests;

namespace RestaurantHomework.Authorization.Api.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotNull().NotEmpty();
        RuleFor(x => x.Password).NotNull().NotEmpty();
    }
}