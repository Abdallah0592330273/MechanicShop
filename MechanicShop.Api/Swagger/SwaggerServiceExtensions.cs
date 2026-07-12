using Asp.Versioning;
using Microsoft.Extensions.Options;
using MechanicShop.WebApi.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MechanicShop.WebApi.Extensions;

public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddSwaggerServices(
        this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion =
                    new ApiVersion(1, 0);

                options.AssumeDefaultVersionWhenUnspecified =
                    true;

                options.ReportApiVersions = true;
                options.ApiVersionReader = new MediaTypeApiVersionReader("X-Api-Version");


                /* new HeaderApiVersionReader(
                        options.ApiVersionReader =
                            ApiVersionReader.Combine(

                               new MediaTypeApiVersionReader(
                                   "api-version")
                                new QueryStringApiVersionReader(
                                    "api-version")

                     
                );
                */
            })
            .AddMvc()
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";

                // false because version isn't part of the URL.
                options.SubstituteApiVersionInUrl = false;
            });

        services.AddEndpointsApiExplorer();

        services.AddTransient<
            IConfigureOptions<SwaggerGenOptions>,
            ConfigureSwaggerOptions>();

        services.AddSwaggerGen();



        return services;
    }

    public static WebApplication UseSwaggerDocumentation(
        this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return app;
        }

        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            var provider =
                app.Services.GetRequiredService<
                    Asp.Versioning.ApiExplorer
                        .IApiVersionDescriptionProvider>();

            foreach (var description
                     in provider.ApiVersionDescriptions.Reverse())
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    $"MechanicShop API " +
                    $"{description.GroupName.ToUpperInvariant()}");
            }

            options.RoutePrefix = "swagger";

            options.DisplayRequestDuration();

            options.EnableTryItOutByDefault();

            options.EnablePersistAuthorization();
        });

        return app;
    }
}