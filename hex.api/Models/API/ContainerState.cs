using hex.api.Enums;
using System;

namespace hex.api.Models.API
{
    /// <summary>
    /// Текущее состояние контейнера
    /// </summary>
    public class ContainerState
    {
        public long Id { get; set; }
        /// <summary>
        /// Номер контейнера
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Тип контейнера
        /// </summary>
        public ContainerType Type { get; set; }
        /// <summary>
        /// Описание контейнера
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Маячок, который стоит на контейнере в данный момент
        /// </summary>
        public long BeaconId { get; set; }
        /// <summary>
        /// Вес контейнера
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// Склад где в данный момент находится контейнер
        /// </summary>
        public long WarehouseId { get; set; }
        /// <summary>
        /// Данные о том, когда контейнер был перемещён на данный склад
        /// </summary>
        public DateTimeOffset MovedTo { get; set; }
        /// <summary>
        /// Данные о том, когда контейнер был замечен на этом месте в последний раз
        /// </summary>
        public DateTimeOffset LastDetected { get; set; }
        /// <summary>
        /// Текущее состояние контейнера
        /// </summary>
        public ContainerStatus Status { get; internal set; }
    }
}
