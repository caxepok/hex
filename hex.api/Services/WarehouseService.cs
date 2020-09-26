using hex.api.Models;
using hex.api.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hex.api.Services
{
    /// <summary>
    /// Сервис взаимодействия с базой
    /// </summary>
    public interface IWarehouseService
    {
        /// <summary>
        /// Возвращает все местоположения контейнеров, которые активны
        /// </summary>
        /// <returns></returns>
        IEnumerable<ContainerPlace> GetActiveContainerPlacesAsync();
        /// <summary>
        /// Возвращет маячок по его идентификатору
        /// </summary>
        /// <param name="gattId">идентификатор маячка (RFID или Bluetooth)</param>
        /// <returns></returns>
        Beacon GetBeacon(string gattId);
        /// <summary>
        /// Возвращает контейнер по его идентификатору
        /// </summary>
        /// <param name="containerId">идентификатор контейнера</param>
        /// <returns></returns>
        Container GetContainer(long containerId);
        /// <summary>
        /// Возвращает контейнер по идентификатору установленного на него маячка
        /// </summary>
        /// <param name="id">идентификатор маячка</param>
        /// <returns></returns>
        Container GetContainerByBeaconId(long id);
        /// <summary>
        /// Возвращает место, где сейчас находится контейнер
        /// </summary>
        /// <param name="id">идентификатор контейнера</param>
        /// <returns></returns>
        ContainerPlace GetContainerPlace(long id);
        IEnumerable<Container> GetContainers();
        IEnumerable<Container> GetCurrentContainers(long warehouseId);
        Warehouse GetWarehouseByObserverSerialNumber(string serialNumber);
        IEnumerable<Warehouse> GetWarehouses();
        ContainerPlace MoveContaier(long containerId, long warehouseId, int placeNumber);
        void UpdateLastDetectedAsync(long id, DateTimeOffset date);
        IEnumerable<ContainerPlace> GetContainerPlacesHistory(long containerId);
    }

    public class WarehouseService : IWarehouseService
    {
        private readonly ILogger _logger;
        private readonly HexDBContext _db;

        public WarehouseService(ILogger<WarehouseService> logger, HexDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public Beacon GetBeacon(string gattId)
        {
            return _db.Beacons.SingleOrDefault(_ => _.GATTId == gattId);
        }

        public IEnumerable<Container> GetContainers()
        {
            return _db.Containers.ToList();
        }

        public Container GetContainerByBeaconId(long id)
        {
            return _db.Containers.SingleOrDefault(_ => _.BeaconId == id);
        }

        public ContainerPlace GetContainerPlace(long id)
        {
            return _db.ContainerPlaces.SingleOrDefault(_ => _.ContainerId == id && _.Finish == null);
        }

        public IEnumerable<Warehouse> GetWarehouses()
        {
            return _db.Warehouses.ToList();
        }

        public IEnumerable<Container> GetCurrentContainers(long warehouseId)
        {
            long[] ids = _db.ContainerPlaces.Where(_ => _.WarehouseId == warehouseId).Select(_ => _.ContainerId).ToArray();
            return _db.Containers.Where(_ => ids.Contains(_.Id)).ToList();  // очень медланная конструкция, но похер
        }

        public ContainerPlace MoveContaier(long containerId, long warehouseId, int placeNumber)
        {
            var now = DateTimeOffset.Now;
            var currentPlace = _db.ContainerPlaces.SingleOrDefault(_ => _.ContainerId == containerId && _.WarehouseId == warehouseId && _.Finish == null);
            if (currentPlace != null)
                currentPlace.Finish = now;

            ContainerPlace newPlace = new ContainerPlace() { ContainerId = containerId, DateFrom = now, LastDetected = now, Number = placeNumber, WarehouseId = warehouseId };
            _db.ContainerPlaces.Add(newPlace);
            _db.SaveChanges();

            return newPlace;
        }

        public Warehouse GetWarehouseByObserverSerialNumber(string serialNumber)
        {
            var observer = _db.Observers.SingleOrDefault(_ => _.SerialNumer == serialNumber);
            if (observer == null)
                return null;
            return _db.Warehouses.SingleOrDefault(_ => _.Id == observer.WarehouseId);
        }

        public void UpdateLastDetectedAsync(long id, DateTimeOffset date)
        {
            var containerPlace = _db.ContainerPlaces.SingleOrDefault(_ => _.ContainerId == id);
            if (containerPlace != null)
                containerPlace.LastDetected = date;

            _db.SaveChanges();
        }

        public IEnumerable<ContainerPlace> GetActiveContainerPlacesAsync()
        {
            return _db.ContainerPlaces.Include(_ => _.Container).Where(_ => _.Finish == null).ToList();
        }

        public Container GetContainer(long containerId)
        {
            return _db.Containers.SingleOrDefault(_ => _.Id == containerId);
        }

        public IEnumerable<ContainerPlace> GetContainerPlacesHistory(long containerId)
        {
            return _db.ContainerPlaces
                .Include(_ => _.Warehouse)
                .Where(_ => _.ContainerId == containerId)
                .OrderByDescending(_ => _.DateFrom)
                .Take(25).ToList();
        }
    }
}
