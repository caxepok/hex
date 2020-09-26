using System;
using System.Collections.Generic;
using System.Text;

namespace hex.common.Models
{
    /// <summary>
    /// Данные телеметрии самого наблюдателя
    /// </summary>
    public class OTrack
    {
        public DateTimeOffset Timestamp { get; set; }
        public string SoftwareVersion { get; set; }  
    }
}
