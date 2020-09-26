using hex.api.Models;
using hex.api.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace hex.api.Services
{
    public interface IWarehouseService
    {
        IEnumerable<ContainerPlace> GetActiveContainerPlacesAsync();
        Beacon GetBeaconAsync(string gattId);
        Container GetContainerAsync(long containerId);
        Container GetContainerByBeaconIdAsync(long id);
        ContainerPlace GetContainerPlaceAsync(long id);
        IEnumerable<Container> GetContainers();
        IEnumerable<Container> GetCurrentContainers(long warehouseId);
        Warehouse GetWarehouseByObserverSerialNumberAsync(string serialNumber);
        IEnumerable<Warehouse> GetWarehouses();
        ContainerPlace MoveContaier(long containerId, long warehouseId, int placeNumber);
        void UpdateLastDetectedAsync(long id, DateTimeOffset date);
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

        public Beacon GetBeaconAsync(string gattId)
        {
            return _db.Beacons.SingleOrDefault(_ => _.GATTId == gattId);
        }

        public IEnumerable<Container> GetContainers()
        {
            return _db.Containers;
        }

        public Container GetContainerByBeaconIdAsync(long id)
        {
            return _db.Containers.SingleOrDefault(_ => _.BeaconId == id);
        }

        public ContainerPlace GetContainerPlaceAsync(long id)
        {
            return _db.ContainerPlaces.SingleOrDefault(_ => _.ContainerId == id);
        }

        public IEnumerable<Warehouse> GetWarehouses()
        {
            return _db.Warehouses;
        }

        public IEnumerable<Container> GetCurrentContainers(long warehouseId)
        {
            long[] ids = _db.ContainerPlaces.Where(_ => _.WarehouseId == warehouseId).Select(_ => _.ContainerId).ToArray();
            return _db.Containers.Where(_ => ids.Contains(_.Id));  // очень медланная конструкция, но похер
        }

        public ContainerPlace MoveContaier(long containerId, long warehouseId, int placeNumber)
        {
            var now = DateTimeOffset.Now;
            var currentPlace = _db.ContainerPlaces.SingleOrDefault(_ => _.ContainerId == containerId && _.WarehouseId == warehouseId);
            if (currentPlace != null)
                currentPlace.Finish = now;

            ContainerPlace newPlace = new ContainerPlace() { ContainerId = containerId, DateFrom = now, LastDetected = now, Number = placeNumber, WarehouseId = warehouseId };
            _db.ContainerPlaces.Add(newPlace);
            _db.SaveChanges();

            return newPlace;
        }

        public Warehouse GetWarehouseByObserverSerialNumberAsync(string serialNumber)
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
            return _db.ContainerPlaces.Where(_ => _.Finish == null);
        }

        public Container GetContainerAsync(long containerId)
        {
            return _db.Containers.SingleOrDefault(_ => _.Id == containerId);
        }
    }
}
