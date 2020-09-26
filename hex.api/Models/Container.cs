using hex.api.Enums;
using System;

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
        /// <summary>
        /// Данные маячка
        /// </summary>

        public virtual Beacon Beacon {get;set;}

        /// <summary>
        /// Текущее содержимое контейнера (цвет)
        /// </summary>
        public string ContentColor
        { 
            get
            {
                switch(Content)
                {
                    case ContainerContent.Empty:
                        return "white";
                    case ContainerContent.Filled:
                        return "forestgreen";
                    case ContainerContent.ReadyAndWeighted:
                        return "gray";
                    case ContainerContent.ReadyToFill:
                        return "wheat";
                    case ContainerContent.Reserved:
                        return "lightgray";
                    default:
                        throw new ArgumentOutOfRangeException("Unknown container content");
                }
            }
        }        /// <summary>
        /// Текущее содержимое контейнера (в виде строки)
        /// </summary>
        public string ContentAsString 
        { 
            get
            {
                switch(Content)
                {
                    case ContainerContent.Empty:
                        return "Пустой";
                    case ContainerContent.Filled:
                        return "Заполнен";
                    case ContainerContent.ReadyAndWeighted:
                        return "Готов и взвешен";
                    case ContainerContent.ReadyToFill:
                        return "Готов к заполнению";
                    case ContainerContent.Reserved:
                        return "Резерв";
                    default:
                        throw new ArgumentOutOfRangeException("Unknown container content");
                }
            }
        }

        /// <summary>
        /// Тип контейнера (в виде строки)
        /// </summary>
        public string TypeAsString
        {
            get
            {
                switch (Type)
                {
                    case ContainerType.Basic:
                        return "Универсальный";
                    case ContainerType.Liquid:
                        return "Жидкости";
                    default:
                        throw new ArgumentOutOfRangeException("Unknown container type");
                }
            }
        }
    }
}
