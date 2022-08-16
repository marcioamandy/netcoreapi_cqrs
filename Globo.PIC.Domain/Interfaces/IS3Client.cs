using System;
using System.Threading;

namespace Globo.PIC.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IS3Client
    {
        /// <summary>
        /// Generate a Pre-Signed URL to temp bucket for the file
        /// </summary>
        /// <param name="key">File name to upload</param>
        /// <param name="contentType">File Content-Type to upload</param>
        /// <param name="verb">HttpVerb</param>
        /// <param name="expires">Datetiem to expire</param>
        /// <returns></returns>
        string GetPreSignedUrl(string key, string fileName, string contentType, string verb, DateTime? expires);

        /// <sumary>
        /// Copy the object from temp-bucket to bucket
        /// </summary>
        /// <param name="key">Object from temp-bucket to copy to bucket</param>
        /// <param name="cancellationToken"></param>
        void CopyObjectToBucket(string key, CancellationToken cancellationToken);

        /// <sumary>
        /// Copy the object from bucket to temp-bucket renaming to tempFileName
        /// </summary>
        /// <param name="key">Object from bucket to copy to temp-bucket</param>
        /// <param name="tempFileName">Temp file name to copy</param>
        /// <param name="cancellationToken"></param>
        void CopyObjectToTemp(string key, string tempFileName, CancellationToken cancellationToken);

        /// <sumary>
        /// Delete object from temp-bucket
        /// </summary>
        /// <param name="key">File name that was deleted</param>
        /// <param name="cancellationToken"></param>
        void DeleteObjectFromTemp(string key, CancellationToken cancellationToken);

        /// <sumary>
        /// Delete object from bucket
        /// </summary>
        /// <param name="key">File name that was deleted</param>
        /// <param name="cancellationToken"></param>
        void DeleteObjectFromBucket(string key, CancellationToken cancellationToken);
    }
}
