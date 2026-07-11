using MechanicShop.WebApi.Extensions;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerServices();

var app = builder.Build();

/*app.UseSwagger(options =>
{
    options.OpenApiVersion =
        OpenApiSpecVersion.OpenApi3_1;
});*/

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerDocumentation();
app.MapControllers();

app.Run();
