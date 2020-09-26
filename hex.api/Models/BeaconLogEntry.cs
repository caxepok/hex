using System;

namespace hex.api.Models
{
    /// <summary>
    /// Данные о регистрации маячка (сырая телеметрия)
    /// </summary>
    public class BeaconLogEntry
    {
        /// <summary>
        /// Идентификтаор маячка
        /// </summary>
        public long BeaconId { get; set; }
        /// <summary>
        /// Идентификтаор устройства, которое "заметило" маячок
        /// </summary>
        public long ObserverId { get; set; }
        /// <summary>
        /// Дата\Время
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
        /// <summary>
        /// Уровень сигнала
        /// </summary>
        public double RSSI { get; set; }
    }
}
