using MessagePack;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace hex.common.Models
{
    /// <summary>
    /// Данные телеметрии контейнера
    /// </summary>
    [MessagePackObject]
    public class CTrack
    {
        /// <summary>
        /// Идентификатор маячка
        /// </summary>
        [Key(0)]
        public string GATTid { get; set; }
        /// <summary>
        /// Штамп времени
        /// </summary>
        [Key(1)]
        public DateTimeOffset Timestamp { get; set; }
        /// <summary>
        /// Мощность передатчика
        /// </summary>
        [Key(2)]
        public double RSSI { get; set; }
    }
}
