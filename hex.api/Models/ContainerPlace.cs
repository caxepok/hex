using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hex.api.Models
{
    /// <summary>
    /// Местонахождение контейнера
    /// </summary>
    public class ContainerPlace
    {
        public long Id { get; set; }
        /// <summary>
        /// Номер места на складе
        /// </summary>
        public long Number { get; set; }
        /// <summary>
        /// Идентификатор контейнера
        /// </summary>
        public long ContainerId { get; set; }
        /// <summary>
        /// Идентификатор склада
        /// </summary>
        public long WarehouseId { get; set; }
        /// <summary>
        /// Дата когда контейнер появился на складе
        /// </summary>
        public DateTimeOffset DateFrom { get; set; }
        /// <summary>
        /// Дата когда контейнер был "замечен" в этом месте в последний раз
        /// </summary>
        public DateTimeOffset LastDetected { get; set; }
        /// <summary>
        /// Дата когда контейнер пропал со склада
        /// </summary>
        public DateTimeOffset? Finish { get; set; }

        public virtual Container Container { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}
