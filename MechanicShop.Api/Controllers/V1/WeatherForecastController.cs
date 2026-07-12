using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace MechanicShop.Api.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [ApiVersion("1.0")]

        [HttpGet]
        public IEnumerable<WeatherForecast> GetWeatherForecast()
        {
            Console.WriteLine("you in version 1 guid congratolation");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

       /* [ApiVersion("2.0")]
        [HttpGet]
        public IEnumerable<WeatherForecast> GetWeatherForecastTwo()
        {
            Console.WriteLine("you in version 2 guid congratolation");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {

                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]

            })
            .ToArray();

        }*/

    }
}
