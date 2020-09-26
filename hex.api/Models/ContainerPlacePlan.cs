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
        /// <summary>
        /// Идентификатор плана
        /// </summary>
        public long Id { get; set; }
        // Идентификатор контейнера
        public long ContaierId { get; set; }
        /// <summary>
        /// Идентификтаор склада, куда следует переместить контейнер по плану
        /// </summary>
        public long WarehouseId { get; set; }
        /// <summary>
        /// Идентификатор сущности перемещения контейнера на склад
        /// </summary>
        public long ContaierPlaceId { get; set; }
        /// <summary>
        /// Причина перемещения контейнера со склада на склад (заполняется оператором)
        /// </summary>
        public string Description { get; set; }
    }
}
