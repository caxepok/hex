using HashtagChris.DotNetBlueZ;
using HashtagChris.DotNetBlueZ.Extensions;
using hex.common.Models;
using hex.observer.common;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hex.observer
{
    /// <summary>
    /// BT сканер для Raspberry Pi
    /// </summary>
    public class RaspberryBTScanner
    {
        private readonly ISignalWriter<CTrack> _writer;
        private readonly string _serialNumber;

        public RaspberryBTScanner(ISignalWriter<CTrack> writer, string serialNumber)
        {
            _writer = writer;
            _serialNumber = serialNumber;
        }

        public async Task StartScan(CancellationToken ct)
        {
            // запускаемся и слушаем окружающие девайсы
            // при обнаружении устройств с нужными GATT характеристиками шлём на сервер пакет о том, что "видим" маячок
            IAdapter1 adapter;

            var adapters = await BlueZManager.GetAdaptersAsync();
            if (adapters.Count == 0)
            {
                throw new Exception("No Bluetooth adapters found.");
            }

            adapter = adapters.First();

            var adapterPath = adapter.ObjectPath.ToString();
            var adapterName = adapterPath.Substring(adapterPath.LastIndexOf("/") + 1);
            Console.WriteLine($"Using Bluetooth adapter {adapterName}");

            // Print out the devices we already know about.
            var devices = await adapter.GetDevicesAsync();
            foreach (var device in devices)
            {
                string deviceDescription = await GetDeviceDescriptionAsync(device);
                Console.WriteLine(deviceDescription);
            }
            Console.WriteLine($"{devices.Count} device(s) found ahead of scan.");

            int newDevices = 0;
            using (await adapter.WatchDevicesAddedAsync(async device =>
            {
                newDevices++;
                // Write a message when we detect new devices during the scan.
                string deviceDescription = await GetDeviceDescriptionAsync(device);
                Console.WriteLine($"[NEW] {deviceDescription}");
            }))
            {
                // запускаем дискаверинг устройств вокруг
                await adapter.StartDiscoveryAsync();
                // ждём до скончания времён или остановки приложения
                await Task.Delay(-1, ct).ContinueWith(t => { });
                await adapter.StopDiscoveryAsync();
            }
        }

        private async Task<string> GetDeviceDescriptionAsync(IDevice1 device)
        {
            var deviceProperties = await device.GetAllAsync();
            // используем Device UUID вместо GATTId в качестве идентификатора маячка, т.к. нет времени заморачиваться
            await _writer.SendInternal(new CTrack() { Timestamp = DateTimeOffset.Now, GATTid = deviceProperties.UUIDs[0], RSSI = deviceProperties.RSSI });
            return $"{deviceProperties.Alias} (Address: {deviceProperties.Address}, RSSI: {deviceProperties.RSSI}), DeviceUUID: {deviceProperties.UUIDs[0]}";
        }
    }
}
