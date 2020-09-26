using hex.api.Enums;

namespace hex.api.Models
{
    /// <summary>
    /// Контейнер
    /// </summary>
    public class Container
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
        /// Текущее содержимое контейнера
        /// </summary>
        public ContainerContent Content { get; set; }
        /// <summary>
        /// Признак наличия канбана
        /// </summary>
        public bool Kanban { get; set; }
    }
}
