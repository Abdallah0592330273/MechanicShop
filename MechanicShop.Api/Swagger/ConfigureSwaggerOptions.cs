using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace MechanicShop.WebApi.Swagger;

public sealed class ConfigureSwaggerOptions(
    IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    : IConfigureOptions<SwaggerGenOptions>
{
    private const string SecuritySchemeId = "bearer";

    public void Configure(SwaggerGenOptions options)
    {
        AddVersionedDocuments(options);
        AddJwtAuthentication(options);
        AddXmlDocumentation(options);

        options.OperationFilter<SwaggerDefaultValues>();

        options.UseInlineDefinitionsForEnums();

        // Prevent duplicate schema names when two classes
        // have the same name in different namespaces.
        options.CustomSchemaIds(type =>
            type.FullName?.Replace('+', '.') ?? type.Name);
    }

    private void AddVersionedDocuments(SwaggerGenOptions options)
    {
        foreach (var description
                 in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = "MechanicShop API",

                    // Example: 1.0 instead of v1
                    Version = description.ApiVersion.ToString(),

                    Description = BuildDescription(description)
                });
        }
    }

    private static void AddJwtAuthentication(
        SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(
            SecuritySchemeId,
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",

                Description =
                    "Enter only the JWT access token. " +
                    "Swagger UI adds the Bearer prefix automatically."
            });

        options.AddSecurityRequirement(
            document => new OpenApiSecurityRequirement
            {
                [
                    new OpenApiSecuritySchemeReference(
                        SecuritySchemeId,
                        document)
                ] = []
            });
    }

    private static void AddXmlDocumentation(
        SwaggerGenOptions options)
    {
        var xmlFileName =
            $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

        var xmlFilePath =
            Path.Combine(AppContext.BaseDirectory, xmlFileName);

        if (File.Exists(xmlFilePath))
        {
            options.IncludeXmlComments(
                xmlFilePath,
                includeControllerXmlComments: true);
        }
    }

    private static string BuildDescription(
        ApiVersionDescription description)
    {
        var apiDescription =
            "Clean Architecture + DDD + CQRS API.";

        if (description.IsDeprecated)
        {
            apiDescription +=
                " This API version is deprecated.";
        }

        return apiDescription;
    }
}