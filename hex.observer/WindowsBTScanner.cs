using hex.common.Models;
using hex.observer.common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace hex.observer
{
    /// <summary>
    /// BT сканер для Windows
    /// </summary>
    public class WindowsBTScanner
    {
        private readonly ISignalWriter<CTrack> _writer;
        private readonly string _serialNumber;

        public WindowsBTScanner(ISignalWriter<CTrack> writer, string serialNumber)
        {
            _writer = writer;
            _serialNumber = serialNumber;
        }

        public async Task StartScan(CancellationToken ct)
        {
            // todo:
        }
    }
}
