using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public const int MAXPAGESIZE = 100;

        /// <summary>
        /// 
        /// </summary>
        [Description("Offset")]
        public int? Offset { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Limit")]
        public int? Limit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseFilter()
        {
            this.Offset = (!this.Offset.HasValue) ? 0 : this.Offset.Value;
            this.Limit = (!this.Limit.HasValue || this.Limit.Value > MAXPAGESIZE) ? MAXPAGESIZE : this.Limit.Value;
        }
    }
}
