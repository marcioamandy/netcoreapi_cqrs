using System;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Globo.PIC.API.Extensions
{

    /// <summary>
    /// 
    /// </summary>
    public static class HangfireConfigExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddHangfireConfig(this IServiceCollection services)
        {
            //Caso local, não habilita
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Local")
                return services;
            
            MySqlStorageOptions mySqlStorageOptions = new MySqlStorageOptions();
 
            string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING_HF");

            services.AddHangfire(opt => opt.UseStorage(
                new MySqlStorage(connectionString, mySqlStorageOptions)
            ));

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        [Obsolete]
        public static void UseHangfireConfig(this IApplicationBuilder app)
        {
            //Caso local, não habilita
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Local") 
                return;

            app.UseHangfireDashboard();

            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                WorkerCount = 1
            });
        }
    }
}
