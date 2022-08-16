using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Net.Http.Headers;
using Globo.PIC.API.Auth;
using Globo.PIC.API.Extensions;
using Globo.PIC.API.Requirements;
using Globo.PIC.API.Claims;
using Globo.PIC.Domain.Models;
using Globo.PIC.Infra.IoC;
using Globo.PIC.Domain.Interfaces;
using Globo.PIC.API.Providers;
using System.Text;
using System.Net.Http;
using System.Net;
using Globo.PIC.API.Configurations;
using Globo.PIC.API.Middlewares;
using Globo.PIC.Infra.Data.ServiceProxies; 

namespace Globo.PIC.API
{
	/// <summary>
	/// 
	/// </summary>
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddHealthChecks();

			services.AddSwaggerGen(s =>
			{
				s.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "pic-teste",
					Description = "Portal de Intenção de Compra - API",
					Contact = new OpenApiContact
					{
						Name = "teste",
						Email = "teste@teste",
						Url = new Uri("https://teste.com.br/")
					},
					License = new OpenApiLicense
					{
						Name = "MIT",
						//Url = new Uri("http://gitlab.teste")
					}
				});
			});

			services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
			{
				builder.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader();
			}));

			services.AddHttpContextAccessor();

			services.AddGlobalExceptionHandlerMiddleware();

			services.AddLogging(loggingBuilder =>
			{
				loggingBuilder.AddConfiguration(Configuration.GetSection("Logging"));
				loggingBuilder.AddConsole();
				loggingBuilder.AddDebug();
			});

			services.AddHttpClient("NodeServiceAPI", client =>
			{
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(
					new MediaTypeWithQualityHeaderValue("application/json"));

				string baseURL = Environment.GetEnvironmentVariable("FILE_SERVICE_ENDPOINT");
				client.BaseAddress = new Uri(baseURL);
			});

			services.AddHttpClient("StarServiceAPI", client =>
			{
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(
					new MediaTypeWithQualityHeaderValue("application/json"));

				string baseURL = Environment.GetEnvironmentVariable("STAR_ENDPOINT");
				client.BaseAddress = new Uri(baseURL);
			});

			services.AddHttpClient("ConteudoServiceAPI", client =>
			{
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(
					new MediaTypeWithQualityHeaderValue("application/json"));

				string baseURL = Environment.GetEnvironmentVariable("CONTEUDO_ENDPOINT");
				client.BaseAddress = new Uri(baseURL);
			}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
			{
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
			});

			services.AddHttpClient("OICServiceAPI", client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();

                string baseURL = Environment.GetEnvironmentVariable("OIC_ENDPOINT");
                string user = Environment.GetEnvironmentVariable("OIC_USERNAME");
                string pass = Environment.GetEnvironmentVariable("OIC_PASSWORD");

                byte[] textAsBytes = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", user, pass));
                var base64 = Convert.ToBase64String(textAsBytes);

                client.DefaultRequestHeaders.Add("Authorization", string.Format("Basic {0}", base64));

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.BaseAddress = new Uri(baseURL);
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

			services.AddHttpClient("DLServiceAPI", client =>
			{
                client.DefaultRequestHeaders.Accept.Clear();

                string baseURL = Environment.GetEnvironmentVariable("DL_ENDPOINT");
				string xApiKey = Environment.GetEnvironmentVariable("DL_X_API_KEY");
				//byte[] textAsBytes = Encoding.UTF8.GetBytes(string.Format("{0}", xApiKey));
				//var xApiKeyEncoded = Convert.ToBase64String(textAsBytes);
				client.DefaultRequestHeaders.Add("x-api-key", xApiKey);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(baseURL);
			}).AddHttpMessageHandler<DLDelegatingHandler>();

			services.AddHttpClient("DLToken", client =>
			{
				client.DefaultRequestHeaders.Accept.Clear();
				string baseURL = Environment.GetEnvironmentVariable("DL_AUTH_ENDPOINT");
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
				client.BaseAddress = new Uri(baseURL);
			});			 

			//Configurations
			services.AddSingleton(Configuration.GetSection("S3").Get<S3Configuration>());
			services.AddSingleton(Configuration.GetSection("EmailSettings").Get<EmailSettings>());
			services.AddSingleton<IAuthorizationHandler, MatchUniqueProfileHandler>();
			services.AddDefaultAWSOptions(Configuration.GetAWSOptions());

			//Hangfire DI
			services.AddHangfireConfig();
			services.AddCronSetup();

			// .NET Native DI Abstraction
			RegisterServices(services);

			// configure basic authentication 
			services.AddAuthentication("BasicAuthentication")
				.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

			services.AddAuthorization(options =>
			{
				options.AddPolicy(AppClaimPolicies.MatchProfile, policy =>
					policy.Requirements.Add(new MatchUniqueProfileRequirement()));
			});

			services.AddControllers().AddNewtonsoftJson(options =>
				options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
			);
		}

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        [Obsolete]
        public void Configure(IApplicationBuilder app,
							  IWebHostEnvironment env)
		{

			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			app.UseHttpsRedirection();

			app.UseHealthChecks("/api/infra/healthcheck");

			app.UseRouting();

			app.UseCors(c =>
			{
				c.AllowAnyHeader();
				c.AllowAnyMethod();
				c.AllowAnyOrigin();
			});

			app.UseGlobalExceptionHandlerMiddleware();

			app.UseStaticFiles();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseSwagger(s =>
			{
				s.RouteTemplate = "pic-service-api/swagger/{documentName}/swagger.json";
			});

			app.UseSwaggerUI(s =>
			{
				s.SwaggerEndpoint("/pic-service-api/swagger/v1/swagger.json", "Globo - PIC API v1");
				s.RoutePrefix = "pic-service-api/swagger";
			});


            app.UseHangfireConfig();

        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="services"></param>
		private static void RegisterServices(IServiceCollection services)
		{
			// Adding dependencies from another layers (isolated from Presentation)
			InjectorBootStrapper.RegisterServices(services);
			services.AddHttpContextAccessor();
			services.AddScoped<ITokenDLProxy, TokenDLProxy>();
			services.AddScoped<DLDelegatingHandler>();
			services.AddSingleton<TokenDL>();
			services.AddScoped<IUserProvider, UserProvider>();

		}
	}
}
