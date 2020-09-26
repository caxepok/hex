using hex.common.Models;
using Microsoft.Extensions.Logging;

namespace hex.observer.common
{
    public class CTrackWriter : SignalRWriter<CTrack>
    {
        private readonly string _endPoint;
        protected override string EndPoint => _endPoint;
        public CTrackWriter(ILogger<CTrackWriter> logger, string serialNumber, string endPoint) : base(logger, serialNumber)
        {
            _endPoint = endPoint;
        }
    }
}
