using System;
using System.Collections.Generic;
using Globo.PIC.Domain.Types.Events;
using Globo.PIC.Infra.Hangfire;
using Microsoft.Extensions.DependencyInjection;

namespace Globo.PIC.API.Configurations
{
    public static class CronSetup
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCronSetup(this IServiceCollection services)
        {
            //Caso local, não habilita
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != "Production")
                return services;

            var schedule = new Dictionary<string, Type[]>(
                new[] {
                    new KeyValuePair<string, Type[]>("*/10 * * * *", new Type[] {
                            typeof(OnStatusLineAlterada),
                    }),
                    new KeyValuePair<string, Type[]>("*/11 * * * *", new Type[] {
                            typeof(OnProjetoDLNotificado)
                    }),
                    new KeyValuePair<string, Type[]>("*/12 * * * *", new Type[] {                            
                            typeof(OnTarefaDLNotificado)
                    }),
                    new KeyValuePair<string, Type[]>("*/1 * * * *", new Type[] {                            
                            typeof(OnUsuarioADSynced)
                    }),
                    new KeyValuePair<string, Type[]>(Hangfire.Cron.Daily(), new Type[] {
                        typeof(OnScheduleConteudoSync)
                    })
                 }
            );
 

            return services.AddCronConfig(schedule);
        }

    }
}
