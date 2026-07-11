using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace MechanicShop.WebApi.Swagger;

public sealed class SwaggerDefaultValues : IOperationFilter
{
    public void Apply(
        OpenApiOperation operation,
        OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        operation.Deprecated |= apiDescription.IsDeprecated;

        if (operation.Parameters is null ||
            operation.Parameters.Count == 0)
        {
            return;
        }

        foreach (var parameter
                 in operation.Parameters.OfType<OpenApiParameter>())
        {
            var parameterDescription =
                apiDescription.ParameterDescriptions
                    .FirstOrDefault(description =>
                        string.Equals(
                            description.Name,
                            parameter.Name,
                            StringComparison.OrdinalIgnoreCase));

            if (parameterDescription is null)
            {
                continue;
            }

            parameter.Description ??=
                parameterDescription.ModelMetadata?.Description;

            parameter.Required |=
                parameterDescription.IsRequired;

            if (parameter.Schema is OpenApiSchema schema &&
                schema.Default is null &&
                parameterDescription.DefaultValue is not null)
            {
                schema.Default =
                    JsonSerializer.SerializeToNode(
                        parameterDescription.DefaultValue,
                        parameterDescription.DefaultValue.GetType());
            }
        }
    }
}