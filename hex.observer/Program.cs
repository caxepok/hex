using hex.common.Models;
using hex.observer.common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hex.observer
{
    /// <summary>
    /// Приложение для Raspberry PI для отслеживания контейнеров
    /// Шлёт пакеты телеметрии обо всех устройствах, обнаруженных в радиусе действия
    /// </summary>
    class Program
    {
        private static ISignalWriter<CTrack> CTrackWriter;
        private static ILoggerFactory _loggerFactory;
        private static ILogger<Program> _logger;

        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            ServiceCollection sc = new ServiceCollection();
            sc.AddLogging(configure => configure.AddConsole());
            IServiceProvider serviceProvider = sc.BuildServiceProvider();
            _loggerFactory = serviceProvider.GetService<ILoggerFactory>();

            // серийный номер устройства обнаружения, должен зашиваться в само устройство
            string serialNumber = args.Length > 0 ? args[0] : "observer2";

            // CTrackWriter - отправляет по SignalR пакеты телеметрии на сервер
            CTrackWriter = new CTrackWriter(serialNumber, "http://localhost:5000/hub/observer");

            RaspberryBTScanner scanner = new RaspberryBTScanner(CTrackWriter, serialNumber);
            Task.Run(() => scanner.StartScan(cts.Token));

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
            cts.Cancel();
        }
    }
}