using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantHomework.Authorization.Api.ActionFilters;
using RestaurantHomework.Authorization.Api.Requests;
using RestaurantHomework.Authorization.Api.Responses;
using RestaurantHomework.Authorization.Api.Validators;
using RestaurantHomework.Authorization.Bll.Commands;
using RestaurantHomework.Authorization.Bll.Queries;

namespace RestaurantHomework.Authorization.Api.Controllers;

[ApiController]
[Route("/api/user")]
[ExceptionFilter]
public class UserController
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<GetInfoResponse> GetInfo([FromQuery] string token)
    {
        var command = new GetUserInfoQuery(token);
        var result = await _mediator.Send(command);

        return new GetInfoResponse(
            result.Username,
            result.Email,
            result.Role,
            result.CreatedAt,
            result.UpdatedAt);
    }
    
    [HttpPut]
    [ExceptionFilter]
    public async Task SetRole(SetRoleRequest request)
    {
        var validator = new SetRoleRequestValidator();
        await validator.ValidateAndThrowAsync(request);
        
        var command = new UpdateRoleCommand(request.Email, request.Role);
        await _mediator.Send(command);
    }
}