using hex.api.Models;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Channels;

namespace hex.api.Services
{
    public interface IChannelWriterService<T, Y>
    {
        /// <summary>
        /// Подписка на события
        /// </summary>
        /// <param name="connectionId">идентификатор подключения</param>
        /// <param name="ids">список идентификаторов объектов для подписки</param>
        /// <param name="writer">writer, куда слать события</param>
        void RegisterConnection(string connectionId, Y[] ids, ChannelWriter<T> writer);
        /// <summary>
        /// Отписка от событий
        /// </summary>
        /// <param name="connectionId"></param>
        void UnregisterConnection(string connectionId);
        /// <summary>
        /// Отправка события подписчикам
        /// </summary>
        /// <param name="evt">события</param>
        /// <param name="id">идентификтаор отправляемого объекта</param>
        void WriteToChannel(T evt, Y id);
    }

    /// <inheritdoc cref="IChannelWriterService{T}"/>
    public class ChannelWriterService<T, Y> : IChannelWriterService<T, Y>
    {
        private readonly ConcurrentDictionary<string, ChannelWriter<T>> _channels;
        private readonly ConcurrentDictionary<string, ChannelWriterSubscription<T, Y>> _subscriptions;

        public ChannelWriterService()
        {
            _channels = new ConcurrentDictionary<string, ChannelWriter<T>>();
            _subscriptions = new ConcurrentDictionary<string, ChannelWriterSubscription<T, Y>>();
        }

        public void RegisterConnection(string connectionId, Y[] ids, ChannelWriter<T> writer)
        {
            if (ids != null && ids.Any())
            {
                var subscription = new ChannelWriterSubscription<T, Y>(writer, ids);
                _subscriptions.AddOrUpdate(connectionId, subscription, (cl, sub) =>
                {
                    sub.ChannelWriter?.Complete();
                    return subscription;
                });
            }
            else
            {
                _channels.AddOrUpdate(connectionId, writer, (cl, cw) =>
                {
                    cw?.Complete();
                    return writer;
                });
            }
        }

        public void UnregisterConnection(string connectionId)
        {
            _subscriptions.TryRemove(connectionId, out var subscription);
            _channels.TryRemove(connectionId, out var channelWriter);
            subscription?.ChannelWriter.Complete();
            channelWriter?.Complete();
        }

        public void WriteToChannel(T evt, Y id)
        {
            foreach (var channel in _channels)
                channel.Value.TryWrite(evt);

            foreach (var sub in _subscriptions)
                if (sub.Value.Ids.Contains(id))
                    sub.Value.ChannelWriter.TryWrite(evt);
        }
    }
}
