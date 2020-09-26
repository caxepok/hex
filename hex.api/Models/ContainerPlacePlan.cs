using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hex.api.Models
{
    /// <summary>
    /// Планируемое перемещение контейнера на склад
    /// </summary>
    public class ContainerPlacePlan
    {
        public long Id { get; set; }
        public long ContaierId { get; set; }
        public long WarehouseId { get; set; }
        /// <summary>
        /// Момент перемещения контейнера на склад
        /// </summary>
        public long ContaierPlaceId { get; set; }
        /// <summary>
        /// Причина перемещения контейнера со склада на склад
        /// </summary>
        public string Description { get; set; }
    }
}
