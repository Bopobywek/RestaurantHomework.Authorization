using System.Net;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantHomework.Authorization.Api.Requests;
using RestaurantHomework.Authorization.Api.Responses;
using RestaurantHomework.Authorization.Api.Validators;
using RestaurantHomework.Authorization.Bll.Commands;

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
    public async Task<RegisterResponse> Register(RegisterRequest request)
    {
        var validator = new RegisterRequestValidator();
        await validator.ValidateAndThrowAsync(request);

        var command = new RegisterUserCommand(request.Username, request.Email, request.Password);
        await _mediator.Send(command);
        
        return new RegisterResponse(Message: "Ok");
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await _mediator.Send(command);

        return new LoginResponse("Ok", result.Token);
    }
}