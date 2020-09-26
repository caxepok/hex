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
    public class ContainerController : Controller
    {
        private readonly ILogger<ContainerController> _logger;
        private readonly IWarehouseService _warehouseService;

        public ContainerController(ILogger<ContainerController> logger, IWarehouseService warehouseService)
        {
            _logger = logger;
            _warehouseService = warehouseService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.Containers = _warehouseService.GetContainers();

            return View();
        }

        [HttpGet("{id}")]
        public IActionResult ById(long id)
        {
            ViewBag.Container = _warehouseService.GetContainerAsync(id);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
