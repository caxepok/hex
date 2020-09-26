using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace hex.api.Models
{
    /// <summary>
    /// Данные о регистрации маячка
    /// </summary>
    public class BeaconLogEntry
    {
        public long BeaconId { get; set; }
        public long ObserverId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        /// <summary>
        /// Уровень сигнала
        /// </summary>
        public double RSSI { get; set; }
    }
}
