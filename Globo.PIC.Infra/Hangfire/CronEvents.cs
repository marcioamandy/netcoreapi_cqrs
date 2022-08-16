using System;
using System.Collections.Generic;
using System.Linq;
using Globo.PIC.Domain.Interfaces;
using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Globo.PIC.Infra.Hangfire
{

    /// <summary>
    /// Configura eventos e dispara eveentos de acordo com cada cron pattern configurado.
    /// </summary>
    [DisableConcurrentExecution(timeoutInSeconds: 60)]
    public class CronEvents : ICronEvents
    {

        /// <summary>
		/// 
		/// </summary>
		private readonly ILogger<CronEvents> logger;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_logger"></param>
        /// <param name="_mediator"></param>
        public CronEvents(ILogger<CronEvents> _logger, IMediator _mediator)
        {
            logger = _logger;
            mediator = _mediator;
        }

        /// <summary>
        /// Configura um dicionario de crons pattern com seus respectivos eventos
        /// </summary>
        public void config(Dictionary<string, Type[]> events)
        {
            try
            {
                foreach (var cron in events)                  
                    RecurringJob.AddOrUpdate(cron.Key, () => CallEvent(cron.Value), cron.Key);

                var _log = string.Format("Agendamentos configurados: {0}",
                        events.ToList()
                        .Select(x => x.Key)
                        .Aggregate((x, y) => string.Format("\n{0}\n{1}", x, y)));

                logger.LogInformation(_log);
            }
            catch (Exception ex)
            {
                logger.LogInformation(
                    string.Format("Falha no carregamento dos eventos agendados: \n{0} \n{1}",
                        ex.Message, ex.InnerException?.Message));
            }
        }

        /// <summary>
        /// despacha os eventos de acordo com o tipo ativado.
        /// </summary>
        /// <param name="ts">Assinatura do evento.</param>
        public void CallEvent(Type[] ts)
        {
            foreach (var item in ts)
                mediator.Publish(Activator.CreateInstance(item));            
        }
    }
}
