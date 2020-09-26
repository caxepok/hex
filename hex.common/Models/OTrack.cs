using System;

namespace hex.common.Models
{
    /// <summary>
    /// Данные телеметрии устройства, собирающего телеметрию наблюдателя
    /// </summary>
    public class OTrack
    {
        /// <summary>
        /// Дата события
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
        /// <summary>
        /// Версия прошивки устройства
        /// </summary>
        public string SoftwareVersion { get; set; }
    }
}
