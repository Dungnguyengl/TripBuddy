using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ConfigurationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly Services.ConfigurationService _configService;

        public ConfigController(Services.ConfigurationService configService)
        {
            _configService = configService;
        }

        [HttpGet]
        public IActionResult GetAllConfigurations()
        {
            var config = _configService.LoadConfigurations();
            return Ok(config);
        }

        [HttpGet("{serviceName}")]
        public IActionResult GetConfigurationByServiceName(string serviceName)
        {
            var config = _configService.LoadConfigurations()
                .Where(x => x.Key.StartsWith("global") || x.Key.StartsWith(serviceName))
                .ToDictionary();

            return Ok(config);
        }

        [HttpPost("{section}")]
        public IActionResult AddOrEditConfiguration(string section, [FromBody] Dictionary<string, string> settings)
        {
            var configs = _configService.LoadConfigurations().ToList();
            var global = configs.Where(x => x.Key.StartsWith("global")).ToList();
            var newSetting = settings.Select(x =>
            {
                return new KeyValuePair<string, string>(!x.Key.StartsWith(section) ? $"{section}:{x.Key}" : x.Key, x.Value);
            });
            global.AddRange([.. newSetting]);
            configs.RemoveAll(x => x.Key.StartsWith(section));
            configs.AddRange([.. newSetting]);

            _configService.SaveConfigurations(configs.ToDictionary());
            if (section == "global")
            {
                _configService.PushAllSectionToRabbitMQ();
            }
            else
            {
                _configService.PushConfigurationToRabbitMQ(section);
            }

            return Ok();
        }
    }
}
