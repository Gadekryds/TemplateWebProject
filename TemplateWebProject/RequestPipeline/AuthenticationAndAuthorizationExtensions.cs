using Microsoft.AspNetCore.Authentication;

namespace TemplateWebProject.RequestPipeline;

public static partial class WebApplicationExtensions
{
    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
    {

        builder.Services.AddAuthentication("Bearer")
                        .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:5001";
                options.Audience = "template-webapi";
            });


        return builder;
    }

    public static WebApplicationBuilder AddAuthorization(this WebApplicationBuilder builder)
    {
        return builder;
    }

    public static WebApplication UseAuthentication(this WebApplication app)
    {
        return app;
    }

    public static WebApplication UseAuthorization(this WebApplication app)
    {
        return app;
    }

}
