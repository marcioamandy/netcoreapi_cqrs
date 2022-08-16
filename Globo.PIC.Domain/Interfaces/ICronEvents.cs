using System;
using System.Collections.Generic;

namespace Globo.PIC.Domain.Interfaces
{

    /// <summary>
    /// Configura eventos para cada cron configurado
    /// </summary>
    public interface ICronEvents
    {

        /// <summary>
        /// Configura crons com base na lista de chaves CronPattern/Evento
        /// </summary>
        public void config(Dictionary<string, Type[]> events);
    }
}
