using hex.observer.common;
using System;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace hex.observer.windows
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string DeviceNameToTrack3 = "iPhone Kirill";    // название устройства которое выступает в качестве маячка
        private const string DeviceNameToTrack5 = "Mi Band 3";    // название устройства которое выступает в качестве маячка
        private const string serialNumber = "observer2";            // серийный номер отслеживающего устройства
        private string DeviceIdToTrack3 = String.Empty;              // идентификатор устройства для отслеживания
        private string DeviceIdToTrack5 = String.Empty;              // идентификатор устройства для отслеживания
        private const string simulatedGattId3 = "CB50AEAC-15D6-43B8-BDFA-8D8E44E3E1BB";  // идентификатор маячка контейнера 00003, который мы посылаем на сервер
        private const string simulatedGattId5 = "EEBBED5E-7E38-4F2C-A385-F6A8FD2510B1";  // идентификатор маячка контейнера 55006, который мы посылаем на сервер

        DeviceWatcher deviceWatcher;
        CTrackWriter _writer;

        public MainPage()
        {
            this.InitializeComponent();

            _writer = new CTrackWriter(serialNumber, "http://localhost:5000/hub/observer");
            _writer.Connect("CTrackWriter", default);

            _ = DiscoveryCycle();
        }

        public async Task DiscoveryCycle()
        {

            // Additional properties we would like about the device.
            // Property strings are documented here https://msdn.microsoft.com/en-us/library/windows/desktop/ff521659(v=vs.85).aspx
            string[] requestedProperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.IsConnected",
                "System.Devices.Aep.SignalStrength", "System.Devices.SignalStrength", "System.Devices.Aep.Bluetooth.Le.IsConnectable" };

            // BT_Code: Example showing paired and non-paired in a single query.
            string aqsAllBluetoothLEDevices = "(System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\")";

            deviceWatcher =
                    DeviceInformation.CreateWatcher(
                        aqsAllBluetoothLEDevices,
                        requestedProperties,
                        DeviceInformationKind.AssociationEndpoint);

            // Register event handlers before starting the watcher.
            deviceWatcher.Added += DeviceWatcher_Added;
            deviceWatcher.Updated += DeviceWatcher_Updated;
            deviceWatcher.Removed += DeviceWatcher_Removed;
            deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
            deviceWatcher.Stopped += DeviceWatcher_Stopped;

            deviceWatcher.Start();
        }

        private void DeviceWatcher_Stopped(DeviceWatcher sender, object args)
        {
            deviceWatcher.Start();
        }

        private void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            deviceWatcher.Stop();
        }

        private void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
        }

        private async void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate deviceInfo)
        {
            if (!deviceInfo.Properties.ContainsKey("System.Devices.Aep.SignalStrength") || deviceInfo.Properties["System.Devices.Aep.SignalStrength"] == null)
                return;
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                lock (this)
                {
                    int rssi = Math.Abs((int)deviceInfo.Properties["System.Devices.Aep.SignalStrength"]);
                    if (deviceInfo.Id == DeviceIdToTrack3)
                        Track3(deviceInfo.Id, rssi);
                    if (deviceInfo.Id == DeviceIdToTrack5)
                        Track5(deviceInfo.Id, rssi);

                }

            });
        }

        private async void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation deviceInfo)
        {
            // We must update the collection on the UI thread because the collection is databound to a UI element.
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                lock (this)
                {
                    int rssi = Math.Abs((int)deviceInfo.Properties["System.Devices.Aep.SignalStrength"]);
                    if (deviceInfo.Name == DeviceNameToTrack3)
                        Track3(deviceInfo.Id, rssi);
                    if (deviceInfo.Name == DeviceNameToTrack5)
                        Track5(deviceInfo.Id, rssi);
                }
            });
        }

        private void Track3(string id, int rssi)
        {
            DeviceIdToTrack3 = id;
            tbRSSI3.Text = rssi.ToString()[0].ToString(); // :)
            _writer.SendInternal(new hex.common.Models.CTrack() { Timestamp = DateTimeOffset.Now, GATTid = simulatedGattId3, RSSI = rssi });
        }

        private void Track5(string id, int rssi)
        {
            DeviceIdToTrack5 = id;
            tbRSSI5.Text = rssi.ToString()[0].ToString(); // :)
            _writer.SendInternal(new hex.common.Models.CTrack() { Timestamp = DateTimeOffset.Now, GATTid = simulatedGattId5, RSSI = rssi });
        }
    }
}
