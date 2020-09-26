using hex.api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace hex.api.Controllers
{
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IContainerStateService _containerStateService;

        public WarehouseController(ILogger<WarehouseController> logger, IContainerStateService containerStateService)
        {
            _logger = logger;
            _containerStateService = containerStateService;
        }

        [HttpGet("state")]
        public IActionResult GetAll()
        {
            return Ok(_containerStateService.GetAll());
        }
    }
}
