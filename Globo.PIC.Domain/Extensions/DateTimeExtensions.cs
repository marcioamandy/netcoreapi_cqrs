using System;

namespace Globo.PIC.Domain.Extensions
{

    /// <summary>
    /// 
    /// </summary>
    public static class DateTimeExtensions
    {

        /// <summary>
        /// Retorna String completa com os formatos curtos de data e hora.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCompleteDateTime(this DateTime? value) =>
            value.HasValue ? value.Value.ToCompleteDateTime() : string.Empty;

        /// <summary>
        /// Retorna String completa com os formatos curtos de data e hora.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCompleteDateTime(this DateTime value) => value.ToString("dd/MM/yyyy HH:mm:ss");

    }
}
