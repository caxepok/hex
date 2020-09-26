namespace hex.api.Models
{
    /// <summary>
    /// Склад
    /// </summary>
    public class Warehouse
    {
        public long Id { get; set; }
        /// <summary>
        /// Название склада
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Широта местонахождения склада
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// Долгота местоположения склада
        /// </summary>
        public double Lng { get; set; }
        /// <summary>
        /// Признак того, что контейнер находится под открытым небом
        /// </summary>
        public bool IsOpenSky { get; set; }
        /// <summary>
        /// Количество мест под контейнеры на складе
        /// </summary>
        public int PlaceCount { get; set; }
    }
}
