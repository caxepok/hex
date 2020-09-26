using hex.api.Models;
using hex.api.Models.API;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace hex.api.Services
{
    /// <summary>
    /// Сервис по отслеживанию контейнеров
    /// </summary>
    public interface IContainerStateService
    {
        Task UpdateContainer(string observerSerialNumber, string gattId, double rssi, int number);
        IEnumerable<ContainerState> GetAll();
    }

    public class ContainerStateService : BackgroundService, IContainerStateService
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private ConcurrentDictionary<long, ContainerState> _containers = new ConcurrentDictionary<long, ContainerState>();

        public ContainerStateService(ILogger<ContainerStateService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task UpdateContainer(string observerSerialNumber, string gattId, double rssi, int number)
        {
            using var scope = _serviceProvider.CreateScope();
            var _warehouseService = scope.ServiceProvider.GetRequiredService<IWarehouseService>();
            var now = DateTimeOffset.Now;
            // определяем от какого устройства пришёл пакет
            Warehouse warehouse = _warehouseService.GetWarehouseByObserverSerialNumberAsync(observerSerialNumber);
            if (warehouse == null)
                return;
            // находим маячок в репозитории маячков
            Beacon beacon = _warehouseService.GetBeaconAsync(gattId);
            if (beacon == null)
                return;
            // проверяем к чему прикреплён маячок
            Container container = _warehouseService.GetContainerByBeaconIdAsync(beacon.Id);
            if (container == null)
                return;

            // проверяем где сейчас контейнер
            ContainerPlace containerPlace = _warehouseService.GetContainerPlaceAsync(container.Id);
            if (containerPlace == null)
            {
                // мы не знаем где этот контейнер
                // поместим его на этот склад
                _warehouseService.MoveContaier(container.Id, warehouse.Id, number);
            }
            else
            {
                // мы знаем где этот контейнер, удостоверимся что он там, где был в прошлый раз
                if (containerPlace.WarehouseId == warehouse.Id && containerPlace.Number != number)
                {
                    // он там где был в последний раз, обновим метку времени
                    _warehouseService.UpdateLastDetectedAsync(containerPlace.Id, now);
                }
                else
                {
                    // контейнер был перемещён
                    _warehouseService.MoveContaier(containerPlace.Id, warehouse.Id, number);
                }
            }


            // мы поняли что это за контейнер, можем получить его состояние
            if (!_containers.TryGetValue(container.Id, out var containerState))
            {
                CreateContainerState(containerPlace, container);
            }
            else
            {
                containerState.LastDetected = now;
                containerState.Status = Enums.ContainerStatus.Online;
                containerState.WarehouseId = warehouse.Id;
            }
        }

        public IEnumerable<ContainerState> GetAll()
        {
            return _containers.Values;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var _warehouseService = scope.ServiceProvider.GetRequiredService<IWarehouseService>();
            // загрузим все текущие состояния контейнеров
            var containerPlaces = _warehouseService.GetActiveContainerPlacesAsync();
            foreach (ContainerPlace cp in containerPlaces)
            {
                Container c = _warehouseService.GetContainerAsync(cp.ContainerId);
                ContainerState containerState = CreateContainerState(cp, c);
            }

            _ = DoWork(stoppingToken);
        }

        private ContainerState CreateContainerState(ContainerPlace cp, Container c)
        {
            var now = DateTimeOffset.Now;
            ContainerState containerState = new ContainerState()
            {
                Id = c.Id,
                Description = c.Description,
                LastDetected = cp.LastDetected,
                MovedTo = cp.DateFrom,
                Number = c.Number,
                Type = c.Type,
                WarehouseId = cp.WarehouseId,
                Weight = c.Weight,
                Status = now.Subtract(cp.LastDetected).TotalMinutes < 1 ? Enums.ContainerStatus.Online : Enums.ContainerStatus.Expired
            };
            _containers.AddOrUpdate(containerState.Id, containerState, (id, oldCs) => containerState);
            return containerState;
        }

        private async Task DoWork(CancellationToken ct)
        {
            while (ct.IsCancellationRequested)
            {
                try
                {

                }
                catch (Exception ex)
                {

                }
                await Task.Delay(5000);
            }
        }
    }
}
