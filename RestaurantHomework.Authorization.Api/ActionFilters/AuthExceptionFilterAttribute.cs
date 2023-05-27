using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RestaurantHomework.Authorization.Bll.Exceptions;

namespace RestaurantHomework.Authorization.Api.ActionFilters;

public class AuthExceptionFilterAttribute: Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        ContentResult result;
        switch (context.Exception)
        {
            case ValidationException validationException:
                result = new ContentResult
                {
                    Content = validationException.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                context.Result = result;
                return;
            case UserAlreadyExistsException userExistsException:
                result = new ContentResult
                {
                    Content = userExistsException.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                context.Result = result;
                return;
            case IncorrectDataException incorrectDataException:
                result = new ContentResult
                {
                    Content = incorrectDataException.Message,
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
                context.Result = result;
                return;
        }
    }
}