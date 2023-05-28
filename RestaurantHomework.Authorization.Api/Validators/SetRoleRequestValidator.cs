using FluentValidation;
using RestaurantHomework.Authorization.Api.Requests;

namespace RestaurantHomework.Authorization.Api.Validators;

public class SetRoleRequestValidator : AbstractValidator<SetRoleRequest>
{
    public SetRoleRequestValidator()
    {
        var roles = new string[] {"customer", "manager", "chef"};
        RuleFor(x => x.Role).Must(x => roles.Contains(x));
        RuleFor(x => x.Email).NotNull().NotEmpty();
    }
}