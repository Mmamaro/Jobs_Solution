using AssessmentAPI.Models;
using AssessmentAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace AssessmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherRepository _weatherRepository;

        public WeatherController(WeatherRepository weatherRepository)
        {
           _weatherRepository = weatherRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Weather>>> GetAllWeather()
        {
            IEnumerable weather = await _weatherRepository.GetAllWeatherAsync();

            return Ok(weather);
        }
    }
}
