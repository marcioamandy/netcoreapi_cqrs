using System;
using System.Threading;
using System.Threading.Tasks;
using Globo.PIC.Domain.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Globo.PIC.Infra.Hangfire
{
    /// <summary>
    /// 
    /// </summary>
    internal class HostedService : IHostedService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IHostApplicationLifetime _appLifetime;

        /// <summary>
        /// 
        /// </summary>
        private readonly ICronEvents _cronEvents;

        /// <summary>
        /// 
        /// </summary>
        private readonly CronConfigEvents _events;

        public HostedService(
            IHostApplicationLifetime appLifetime,
            CronConfigEvents events,
            ICronEvents cronEvents)
        {
            _appLifetime = appLifetime;
            _cronEvents = cronEvents;
            _events = events;
        }

        void OnStarted()
        {
            if (_events == null)
                throw new Exception(
                    "É necessário atribuir uma instância válida do tipo ConfigCronEvents para utilização de Eventos Agendados.");


            if(_events.Events == null)
                throw new Exception(
                   "Configure um dicionario de Eventos para utilização de Eventos Agendados.");


            _cronEvents.config(_events.Events);
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);

            return Task.CompletedTask;
        }

        Task IHostedService.StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
        
    }
}