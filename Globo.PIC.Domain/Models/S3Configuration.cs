using System.ComponentModel;

namespace Globo.PIC.Domain.Models
{
    /// <summary>
    /// Claas with the configuration of S3
    /// </summary>
    public class S3Configuration
    {

        /// <summary>
        /// Path where original files are moved on the Bucket
        /// </summary>
        [Description("Folder Path")]
        public string FolderPath { get; set; }

        /// <summary>
        /// Path where compressed files are moved on the Bucket
        /// </summary>
        [Description("Compressed Folder Path")]
        public string CompressedFolderPath { get; set; }

        /// <summary>
        /// Extension of compressed files
        /// </summary>
        [Description("Compressed Extension")]
        public string CompressedExtension { get; set; }

        /// <summary>
        /// Path used to download files on the temp bucket
        /// </summary>
        [Description("Downloads Temp Path")]
        public string DownloadsTempPath { get; set; }
    }
}
