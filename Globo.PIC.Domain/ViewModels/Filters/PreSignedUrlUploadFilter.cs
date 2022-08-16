
using System.ComponentModel;

namespace Globo.PIC.Application.Filters
{
    public class PreSignedUrlUploadFilter
    {
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
    }
}

