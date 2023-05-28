using System.Net;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics;
using RestaurantHomework.Authorization.Api.NamingPolicies;
using RestaurantHomework.Authorization.Bll.Extensions;
using RestaurantHomework.Authorization.Dal.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddBll(builder.Configuration)
    .AddDalInfrastructure(builder.Configuration)
    .AddDalRepositories();

var services = builder.Services;
services.AddFluentValidation(conf =>
{
    conf.RegisterValidatorsFromAssembly(typeof(Program).Assembly);
    conf.AutomaticValidationEnabled = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(c => c.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
    var response = new { ErrorMessage = exception?.Message };

    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

    await context.Response.WriteAsJsonAsync(response);

}));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MigrateUp();

app.Run();