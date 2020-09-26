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
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
