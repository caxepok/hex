namespace hex.api.Enums
{
    /// <summary>
    /// Текущее состояние контейнера
    /// </summary>
    public enum ContainerStatus
    {
        /// <summary>
        /// Неизвестное
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Истекла дата отслеживания
        /// </summary>
        Expired = 1,
        /// <summary>
        /// Замечен недавно
        /// </summary>
        Online = 2
    }
}
