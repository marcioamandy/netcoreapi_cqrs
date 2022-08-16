using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Globo.PIC.Infra.Hangfire
{
    public static class CronConfigExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCronConfig(this IServiceCollection services, Dictionary<string, Type[]> events)
        {
            services.AddSingleton(new CronConfigEvents(events));

            services.AddHostedService<HostedService>();

            return services;
        }
    }
}
