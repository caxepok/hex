using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hex.api.Models
{
    /// <summary>
    /// Маячок
    /// </summary>
    public class Beacon
    {
        public long Id { get; set; }
        /// <summary>
        /// Дата когда была заменена батарея
        /// </summary>
        public DateTimeOffset BatteryReplaced { get; set; }
        /// <summary>
        /// Идентификатор GATT
        /// </summary>
        public string GATTId { get; set; }
    }
}
