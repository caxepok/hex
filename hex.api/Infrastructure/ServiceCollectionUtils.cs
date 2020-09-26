using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace hex.api.Infrastructure
{
    public static class ServiceCollectionUtils
    {
        public static void AddHostedService<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, IHostedService, TService
        {
            services.AddSingleton<TService, TImplementation>();
            services.AddHostedService<HostedServiceWrapper<TService>>();
        }

        private class HostedServiceWrapper<TService> : IHostedService
        {
            private readonly IHostedService _hostedService;

            public HostedServiceWrapper(TService hostedService)
            {
                _hostedService = (IHostedService)hostedService;
            }

            public Task StartAsync(CancellationToken cancellationToken)
                => _hostedService.StartAsync(cancellationToken);

            public Task StopAsync(CancellationToken cancellationToken)
                => _hostedService.StopAsync(cancellationToken);
        }
    }

}
