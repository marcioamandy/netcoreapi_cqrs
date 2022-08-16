using Globo.PIC.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Globo.PIC.API.Middlewares
{

    /// <summary>
    /// 
    /// </summary>
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                IHeaderDictionary headers = null;
                if (context.Request != null && context.Request.Headers != null)
                    headers = context.Request.Headers;

                _logger.LogError($"Falha de authorização: {ex.Message}", $"Cabeçalhos: \n {headers}");
                await HandleExceptionAsync(context, ex, (int)HttpStatusCode.Unauthorized);
            }
            catch (JsonSerializationException ex)
            {
                _logger.LogError($"Falha na serialização do objeto: {ex}");
                await HandleExceptionAsync(context, ex, (int)HttpStatusCode.PreconditionFailed);
            }
            catch (DuplicateNameException ex)
            {
                _logger.LogError($"Ação não permitida: {ex}");
                await HandleExceptionAsync(context, ex, (int)HttpStatusCode.PreconditionFailed);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError($"Registro não encontrado: {ex}");
                await HandleExceptionAsync(context, ex, (int)HttpStatusCode.NotFound);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError($"Falha no processamento: {ex}");
                await HandleExceptionAsync(context, ex, 422);
            }
            catch (ValidationException ex)
            {
                _logger.LogError($"Falha na validação: {ex}");
                await HandleExceptionAsync(context, ex, 422);
            }
            catch (BadRequestException ex)
            {
                _logger.LogError($"Erro na requisição: {ex}");
                await HandleExceptionAsync(context, ex, 400);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro inesperado: {ex}");
                await HandleExceptionAsync(context, ex, (int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            string message = "Ocorreu um erro enquanto processava a requisição.";

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            if(statusCode != 500)
			{
                message = exception.Message;
			}

            var json = new
            {
                success = false,
                statusCode = context.Response.StatusCode,
                message,
                stack = exception
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(json, new JsonSerializerSettings()
            {
                //ReferenceLoopHandling = ReferenceLoopHandling.Ignore 
            }));
        }
    }
}
