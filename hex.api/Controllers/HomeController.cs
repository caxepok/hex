using hex.api.Models;
using hex.api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace hex.api.Controllers
{
    /// <summary>
    /// Основной контроллер интерфейса
    /// </summary>
    [Route("home")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWarehouseService _warehouseService;
        private readonly IContainerStateService _containerStateService;

        public HomeController(ILogger<HomeController> logger, IWarehouseService warehouseService, IContainerStateService containerStateService)
        {
            _logger = logger;
            _warehouseService = warehouseService;
            _containerStateService = containerStateService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.ContainerPlaces = _warehouseService.GetActiveContainerPlacesAsync();
            ViewBag.Warehouses = _warehouseService.GetWarehouses();
            ViewBag.ContainerStates = _containerStateService.GetAll();

            return View();
        }

        [HttpGet("container")]
        public IActionResult Containers()
        {
            ViewBag.Containers = _warehouseService.GetContainers();
            ViewBag.ContainerStates = _containerStateService.GetAll();
            ViewBag.ContainerPlaces = _warehouseService.GetActiveContainerPlacesAsync();
            ViewBag.Warehouses = _warehouseService.GetWarehouses();

            return View();
        }

        [HttpGet("container/{id}")]
        public IActionResult Container(long id)
        {
            ViewBag.Container = _warehouseService.GetContainer(id);
            ViewBag.Places = _warehouseService.GetContainerPlacesHistory(id);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
