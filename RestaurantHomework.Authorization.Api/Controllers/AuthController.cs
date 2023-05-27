using System.Net;
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
[Route("auth")]
public class AuthController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("register")]
    [AuthExceptionFilter]
    public async Task<RegisterResponse> Register(RegisterRequest request)
    {
        var validator = new RegisterRequestValidator();
        await validator.ValidateAndThrowAsync(request);

        var command = new RegisterUserCommand(request.Username, request.Email, request.Password);
        await _mediator.Send(command);
        
        return new RegisterResponse(Message: "Ok");
    }

    [HttpPost]
    [Route("set-role")]
    [AuthExceptionFilter]
    public async Task<string> SetRole(SetRoleRequest request)
    {
        var command = new UpdateRoleCommand(request.Email, request.Role);
        await _mediator.Send(command);
        
        return "Ok";
    }
    
    [HttpPost]
    [Route("login")]
    [AuthExceptionFilter]
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await _mediator.Send(command);

        return new LoginResponse("Ok", result.Token);
    }
    
    [HttpGet]
    [Route("get-info")]
    [AuthExceptionFilter]
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
}