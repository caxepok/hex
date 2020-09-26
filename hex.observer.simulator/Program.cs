using hex.common.Models;
using hex.observer.common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace hex.observer.simulator
{
    /// <summary>
    /// Приложение для Raspberry PI для отслеживания контейнеров (симулятор)
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

            string serialNumber = args.Length > 0 ? args[0] : "observer1";

            CTrackWriter = new CTrackWriter(_loggerFactory.CreateLogger<CTrackWriter>(), serialNumber, "http://localhost:5000/hub/observer");

            Task.Run(() => DoWork(cts.Token));

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }

        private static async Task DoWork(CancellationToken ct)
        {
            CTrackWriter.Connect("CTrackWriter", ct);

            while (!ct.IsCancellationRequested)
            {
                try
                {
                    await CTrackWriter.SendInternal(new CTrack() { GATTid = "00001", RSSI = 60, Timestamp = DateTimeOffset.Now });
                    await CTrackWriter.SendInternal(new CTrack() { GATTid = "00002", RSSI = 60, Timestamp = DateTimeOffset.Now });
                }
                catch (Exception ex)
                {

                }
                await Task.Delay(10000);
            }
        }
    }
}
