using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace hex.observer.common
{
    public interface ISignalWriter<T>
    {
        void Connect(string hubMethodName, CancellationToken ct);
        ValueTask SendInternal(T evt);
    }

    public abstract class SignalRWriter<T> : ISignalWriter<T>
    {
        private static readonly SignalRRetryPolicy _signalRRetryPolicy = new SignalRRetryPolicy();
        private readonly Channel<T> _channel = Channel.CreateUnbounded<T>();
        private readonly string _observerSerialNumber;
        protected abstract string EndPoint { get; }

        public SignalRWriter(string observerSerialNumber)
        {
            _observerSerialNumber = observerSerialNumber;
        }

        public ValueTask SendInternal(T evt) =>
            _channel.Writer.WriteAsync(evt);

        public void Connect(string hubMethodName, CancellationToken ct)
        {
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl(new Uri(EndPoint))
                //.AddMessagePackProtocol(options =>
                //{
                //    options.FormatterResolvers = new List<MessagePack.IFormatterResolver>()
                //    {
                //        MessagePack.Resolvers.StandardResolver.Instance
                //    };
                //})
                .AddJsonProtocol()
                .WithAutomaticReconnect(_signalRRetryPolicy)
                .Build();

            _ = ConnectWithRetryAndReadAsync(EndPoint, hubMethodName, _channel.Reader, connection, ct);
        }

        private async Task ConnectWithRetryAndReadAsync(string endPoint, string hubMethodName, ChannelReader<T> reader, HubConnection connection, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    // first connect
                    await connection.StartAsync(ct);
                    // start streaming
                    while (!ct.IsCancellationRequested)
                    {
                        try
                        {
                            if (connection.State != HubConnectionState.Connected)
                            {
                                await Task.Delay(_signalRRetryPolicy.NextRetryDelay(), ct).ContinueWith(t => { });
                                continue;
                            }
                            await connection.SendAsync(hubMethodName, _observerSerialNumber, reader);
                            break;
                        }
                        catch (TaskCanceledException) when (ct.IsCancellationRequested)
                        {
                            break;
                        }
                        catch (Exception ex)
                        {
                            TimeSpan tsRetry = _signalRRetryPolicy.NextRetryDelay();
                            await Task.Delay(tsRetry, ct).ContinueWith(t => { });
                        }
                    }
                    if (connection.State == HubConnectionState.Connected)
                        break;
                }
                catch (TaskCanceledException) when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    TimeSpan tsRetry = _signalRRetryPolicy.NextRetryDelay();
                    await Task.Delay(tsRetry, ct).ContinueWith(t => { });
                }
            }
        }
    }

    public class SignalRRetryPolicy : IRetryPolicy
    {
        private readonly Random _random = new Random();

        public TimeSpan? NextRetryDelay(RetryContext retryContext) =>
            TimeSpan.FromSeconds(_random.NextDouble() * 10);

        public TimeSpan NextRetryDelay() =>
            TimeSpan.FromSeconds(_random.NextDouble() * 10);
    }
}
