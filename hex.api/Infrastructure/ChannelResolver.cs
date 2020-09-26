using hex.api.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace hex.api.Infrastructure
{
    public interface IChannelResolver<T, Y>
    {
        IChannelWriterService<T, Y> ResolveChannel(string key);
    }

    public class ChannelResolver<T, Y> : IChannelResolver<T, Y>
    {
        private readonly object _lock = new object();
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, IChannelWriterService<T, Y>> _channels = new Dictionary<string, IChannelWriterService<T, Y>>();

        public ChannelResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IChannelWriterService<T, Y> ResolveChannel(string key)
        {
            lock (_lock)
            {
                if (!_channels.TryGetValue(key, out var channelWriterService))
                {
                    channelWriterService = _serviceProvider.GetRequiredService<IChannelWriterService<T, Y>>();
                    _channels.Add(key, channelWriterService);
                }
                return channelWriterService;
            }
        }
    }
}
