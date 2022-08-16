using System;
using System.Collections.Generic;

namespace Globo.PIC.Infra.Hangfire
{
    /// <summary>
    /// Configuração de eventos agendados para Hangfire
    /// </summary>
    public class CronConfigEvents
    {

        /// <summary>
        /// Dicionário de agendamentos utilizando Cron Pattern e seus respectivos Eventos.
        /// </summary>
        public Dictionary<string, Type[]> Events { get; } = new Dictionary<string, Type[]>();

        /// <summary>
        /// 
        /// </summary>
        public CronConfigEvents() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="events">Dicionário de agendamentos utilizando Cron Pattern como chave e seus respectivos Eventos como valor.</param>
        public CronConfigEvents(Dictionary<string, Type[]> events)
        {
            if (events != null) Events = events;
        }

    }
}
