using Microsoft.AspNetCore.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using TemplateWebProject.Errors;

namespace TemplateWebProject.RequestPipeline;

public static class WebApplicationExtensions
{
    private const string _errorExceptionRoute = "/error";
    public static WebApplicationBuilder AddExceptionHandling(this WebApplicationBuilder builder)
    {

        builder.Services.AddProblemDetails(opt =>
        {
            opt.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
                context.ProblemDetails.Extensions["serviceVersion"] = context.HttpContext.RequestServices.GetService<IConfiguration>()?.GetValue<string>("ServiceVersion") ?? "-";
            };
        });
        
        return builder;
    }

    public static WebApplication UseGlobalErrorHandling(this WebApplication app)
    {
        app.UseExceptionHandler(_errorExceptionRoute);

        app.Map(_errorExceptionRoute, (HttpContext httpContext) =>
        {
            Exception? exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (exception is null)
            {
                return Results.Problem();
            }

            return exception switch
            {
                ServiceException serviceException => Results.Problem(statusCode: serviceException.StatusCode, detail: serviceException.ExceptionMessage),
                _ => Results.Problem()
            };
        });

        return app;
    }
}
