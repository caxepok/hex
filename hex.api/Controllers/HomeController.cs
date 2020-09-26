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

        public IActionResult Index()
        {
            ViewBag.ContainerPlaces = _warehouseService.GetActiveContainerPlacesAsync().ToList();
            ViewBag.Warehouses = _warehouseService.GetWarehouses().ToList();
            ViewBag.ContainerStates = _containerStateService.GetAll().ToList();

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
