using System.ComponentModel;

namespace Globo.PIC.Domain.ViewModels
{

    /// <summary>
    /// 
    /// </summary>
    public class PreSignedUrlViewModel
    {

        /// <summary>
        /// Temporary File Name to download from bucket
        /// </summary>
        [Description("Temporary File Name")]
        public string TempFileName { get; set; }

        /// <summary>
        /// Pre-Signed URL to download file from bucket
        /// </summary>
        [Description("Pre-Signed URL")]
        public string Url { get; set; }

        /// <summary>
        /// Error Message on get Pre-Signed URL
        /// </summary>
        [Description("Error Message")]
        public string Error { get; set; }
    }
}
