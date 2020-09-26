using hex.api.Models;
using hex.api.Models.API;
using hex.api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace hex.api.Controllers
{

    /// <summary>
    /// REST API Контроеллер для интеграции с внешними системами
    /// </summary>
    [ApiController]
    public class IntegrationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IWarehouseService _warehouseService;
        private readonly IContainerStateService _containerStateService;

        public IntegrationController(ILogger<IntegrationController> logger, IWarehouseService warehouseService, IContainerStateService containerStateService)
        {
            _logger = logger;
            _warehouseService = warehouseService;
            _containerStateService = containerStateService;
        }

        /// <summary>
        /// Возвращает все контейнеры, заведённые в системе
        /// </summary>
        [HttpGet("container")]
        public IActionResult GetContainers()
        {
            return Ok(_warehouseService.GetContainers());
        }

        /// <summary>
        /// Возвращает состояния всех контейнеров
        /// </summary>
        [HttpGet("container/state")]
        public IActionResult GetContainersState()
        {
            return Ok(_containerStateService.GetAll());
        }

        /// <summary>
        /// Возвращает контейнер по его идентификатору
        /// </summary>
        /// <param name="id">идентификтаор контейнера</param>
        [HttpGet("container/{id}")]
        public IActionResult GetContainer(long id)
        {
            Container container = _warehouseService.GetContainer(id);
            if (container == null)
                return NotFound();
            return Ok(container);
        }

        /// <summary>
        /// Возвращает все контейнеры, заведённые в системе
        /// </summary>
        [HttpGet("container/{id}/state")]
        public IActionResult GetContainerState(long id)
        {
            ContainerState state = _containerStateService.GetById(id);
            if (state == null)
                return NotFound();
            return Ok(state);
        }

        /// <summary>
        /// Возвращает историю перемещения контейнера
        /// </summary>
        /// <param name="id">идентификатор контейнера</param>
        /// <returns></returns>
        [HttpGet("container/{id}/history")]
        public IActionResult GetContainerHistory(long id)
        {
            return Ok(_warehouseService.GetContainerPlacesHistory(id));
        }

        /// <summary>
        /// Возвращает список складов, заведённых в системе
        /// </summary>
        [HttpGet("warehouse")]
        public IActionResult GetWarehouse()
        {
            return Ok(_warehouseService.GetWarehouses());
        }

        /// <summary>
        /// Возвращает все контейнеры, находящиеся сейчас на складе
        /// </summary>
        /// <param name="id">идентификатор склада</param>
        /// <returns></returns>
        [HttpGet("warehouse/containers")]
        public IActionResult GetContainersAtWarehouse(long id)
        {
            return Ok(_containerStateService.GetAll().Where(_ => _.WarehouseId == id));
        }
    }
}
