using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace hex.api.Models
{
    /// <summary>
    /// Данные о подписке на уведомления по SignalR
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Y"></typeparam>
    public class ChannelWriterSubscription<T, Y>
    {
        public ChannelWriter<T> ChannelWriter { get; set; }
        public HashSet<Y> Ids { get; set; }

        public ChannelWriterSubscription(ChannelWriter<T> channelWriter, Y[] ids)
        {
            ChannelWriter = channelWriter;
            Ids = new HashSet<Y>();
            foreach (Y id in ids.Distinct())
                Ids.Add(id);
        }
    }
}
