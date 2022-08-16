using MediatR;
using System;
using System.ComponentModel; 

namespace Globo.PIC.Domain.Types.Queries
{

    /// <summary>
    /// 
    /// </summary>
    public class GetPreSignedUrl : IRequest<string>
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Key")]
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("File Name")]
        public string FileName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Content Type")]
        public string ContentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Verb")]
        public string Verb { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Expires")]
        public DateTime? Expires { get; set; }

    }
}
