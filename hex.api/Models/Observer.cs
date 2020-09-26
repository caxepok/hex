using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hex.api.Models
{
    /// <summary>
    /// Наблюдатель маячков
    /// </summary>
    public class Observer
    {
        public long Id { get; set; }
        /// <summary>
        /// Название хоста
        /// </summary>
        public string Hostname { get; set; }
        /// <summary>
        /// Идентификатор склада где стоит наблюдатель
        /// </summary>
        public long WarehouseId { get; set; }
        /// <summary>
        /// Серийны номер приложения наблюдателя
        /// </summary>
        public string SerialNumer { get; set; }
        /// <summary>
        /// Версия приложения
        /// </summary>
        public string SoftwareVersion { get; set; }
    }
}
