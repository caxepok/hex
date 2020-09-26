using hex.api.Infrastructure;
using hex.api.Services;
using hex.common.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace hex.api.Hubs
{
    public class ObserverHub : Hub
    {
        private readonly ILogger _logger;
        private readonly IChannelWriterService<CTrack, string> _cTrack;
        private readonly IContainerStateService _containerStateService;

        public ObserverHub(ILogger<ObserverHub> logger, IChannelResolver<CTrack, string> channelResolver, IContainerStateService containerStateService)
        {
            _logger = logger;
            _cTrack = channelResolver.ResolveChannel("ctrack");
            _containerStateService = containerStateService;
        }

        public ChannelReader<CTrack> SubscribeInterval(string[] ids, CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => _cTrack.UnregisterConnection(Context.ConnectionId));

            _logger.LogTrace("ctrack: subscribed client id: {0}", Context.ConnectionId);
            var channelReader = Channel.CreateUnbounded<CTrack>();
            _cTrack.RegisterConnection(Context.ConnectionId, ids, channelReader.Writer);
            return channelReader;
        }

        public async Task CTrackWriter(string serialNumber, IAsyncEnumerable<CTrack> stream)
        {
            await foreach (var item in stream)
            {
                _logger.LogInformation($"ctrack: {item.GATTid}");
                try
                {
                    // todo: тут нужна асинхронность, при больших потоках событий всё будет лагать и ляжет
                    await _containerStateService.UpdateContainer(serialNumber, item.GATTid, item.RSSI, (int)item.RSSI / 10);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to process ctrack message");
                }
            }
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogTrace("ctrack: connected client: {0}", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogTrace("ctrack: disconnected client: {0}", Context.ConnectionId);
            _cTrack.UnregisterConnection(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
