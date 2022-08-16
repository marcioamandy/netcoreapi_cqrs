using System;

namespace Globo.PIC.Domain.Extensions
{    
    /// <summary>
    /// 
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Default error message prefix
        /// </summary>
        private const string ERROR_MESSAGE = "An error has occurred";        

        /// <summary>
        /// Formats exception error message concating all Inner Exceptions messages and Stack Trace
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string FormatErrorMessage(this Exception ex)
        {
            return $"{ERROR_MESSAGE}: {ex.Message} {ex.GetInnerExceptionMessages()} \n {ex.StackTrace}";
        }

        /// <summary>
        /// Return all inner exceptions messages
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        private static string GetInnerExceptionMessages(this Exception ex)
        {
            var msg = "";
            var exception = ex;

            while (exception.InnerException != null)
            {
                msg = $"{msg} \n {exception.InnerException.Message}";

                exception = exception.InnerException;
            }

            return msg;
        }               
    }
}