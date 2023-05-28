using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestaurantHomework.Authorization.Bll.Exceptions;

namespace RestaurantHomework.Authorization.Api.ActionFilters;

public class ExceptionFilterAttribute: Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        ContentResult result;
        switch (context.Exception)
        {
            case ValidationException validationException:
                var response = new { ErrorMessage = validationException.Message };
                result = new ContentResult
                {
                    Content = JsonSerializer.Serialize(response),
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                context.Result = result;
                return;
            case UserAlreadyExistsException:
            case IncorrectDataException:
                var responseCustomException = new { ErrorMessage = context.Exception.Message };
                result = new ContentResult
                {
                    Content = JsonSerializer.Serialize(responseCustomException),
                    ContentType = "application/json",
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                context.Result = result;
                return;
        }
    }
}