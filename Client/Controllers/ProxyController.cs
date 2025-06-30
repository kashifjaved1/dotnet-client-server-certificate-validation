using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _config;

        public ProxyController(IHttpClientFactory clientFactory, IConfiguration config)
        {
            _clientFactory = clientFactory;
            _config = config;
        }

        [HttpGet("weather")]
        public async Task<IActionResult> GetWeather()
        {
            var client = _clientFactory.CreateClient("ServerClient");
            var serverUrl = _config["Server:BaseUrl"];

            var response = await client.GetAsync($"{serverUrl}/weatherforecast");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }
    }
}
